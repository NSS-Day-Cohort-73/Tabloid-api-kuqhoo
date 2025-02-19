using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tabloid.Models;
using Tabloid.Models.DTOs;
using Tabloid.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Tabloid.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostController : ControllerBase
{
    private TabloidDbContext _dbContext;

    public PostController(TabloidDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try 
        {
            List<PostDTO> posts = _dbContext.Posts
                .Include(p => p.UserProfile)
                    .ThenInclude(up => up.IdentityUser)
                .Include(p => p.Category)
                .Where(p => p.IsApproved && p.CreatedAt <= DateTime.Now)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    HeaderImage = p.HeaderImage,
                    IsApproved = p.IsApproved,
                    CategoryName = p.Category.Name,
                    Author = new UserProfileDTO
                    {
                        Id = p.UserProfile.Id,
                        FirstName = p.UserProfile.FirstName,
                        LastName = p.UserProfile.LastName,
                        Email = p.UserProfile.IdentityUser.Email,
                        UserName = p.UserProfile.IdentityUser.UserName,
                        ImageLocation = p.UserProfile.ImageLocation
                    }
                })
                .ToList();

            return Ok(posts);
        }
        catch
        {
            return StatusCode(500, "An error occurred while retrieving posts.");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePost(int id)
    {
        Post postToDelete = _dbContext.Posts
            .SingleOrDefault((p) => p.Id == id);
        _dbContext.Posts.Remove(postToDelete);
        _dbContext.SaveChanges();
        return NoContent();

    }

    [HttpPost]
    public IActionResult CreatePost(Post post)
    {
        post.IsApproved = true;
        post.CreatedAt = DateTime.Now;

        _dbContext.Posts.Add(post);
        _dbContext.SaveChanges();
        return Created($"/api/post/{post.Id}", post);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try 
        {
            PostDTO post = _dbContext.Posts
                .Include(p => p.UserProfile)
                    .ThenInclude(up => up.IdentityUser)
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .Select(p => new PostDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    HeaderImage = p.HeaderImage,
                    IsApproved = p.IsApproved,
                    CategoryName = p.Category.Name,
                    Author = new UserProfileDTO
                    {
                        Id = p.UserProfile.Id,
                        FirstName = p.UserProfile.FirstName,
                        LastName = p.UserProfile.LastName,
                        Email = p.UserProfile.IdentityUser.Email,
                        UserName = p.UserProfile.IdentityUser.UserName,
                        ImageLocation = p.UserProfile.ImageLocation
                    }
                })
                .FirstOrDefault();

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }
        catch
        {
            return StatusCode(500, "An error occurred while retrieving the post.");
        }
    }

    // Get posts by specific user (for readers)
    [HttpGet("user/{id}")]
    public IActionResult GetByUser(int id)
    {
        try
        {
            List<PostDTO> posts = _dbContext.Posts
                .Include(p => p.UserProfile)
                    .ThenInclude(up => up.IdentityUser)
                .Include(p => p.Category)
                .Where(p => p.UserProfileId == id && p.IsApproved)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    HeaderImage = p.HeaderImage,
                    CategoryName = p.Category.Name,
                    Author = new UserProfileDTO
                    {
                        Id = p.UserProfile.Id,
                        FirstName = p.UserProfile.FirstName,
                        LastName = p.UserProfile.LastName,
                        UserName = p.UserProfile.IdentityUser.UserName,
                        ImageLocation = p.UserProfile.ImageLocation
                    }
                })
                .ToList();

            return Ok(posts);
        }
        catch
        {
            return StatusCode(500, "An error occurred while retrieving the user's posts.");
        }
    }

    // Get my posts (for authors)
    [HttpGet("my")]
    public IActionResult GetMyPosts()
    {
        try
        {
            string identityUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            UserProfile userProfile = _dbContext.UserProfiles
                .SingleOrDefault(up => up.IdentityUserId == identityUserId);

            if (userProfile == null)
            {
                return NotFound("User profile not found");
            }

            List<PostDTO> posts = _dbContext.Posts
                .Include(p => p.Category)
                .Include(p => p.UserProfile)
                    .ThenInclude(up => up.IdentityUser)
                .Where(p => p.UserProfileId == userProfile.Id)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    HeaderImage = p.HeaderImage,
                    IsApproved = p.IsApproved,
                    CreatedAt = p.CreatedAt,
                    CategoryName = p.Category.Name,
                    Author = new UserProfileDTO
                    {
                        Id = p.UserProfile.Id,
                        FirstName = p.UserProfile.FirstName,
                        LastName = p.UserProfile.LastName,
                        UserName = p.UserProfile.IdentityUser.UserName,
                        ImageLocation = p.UserProfile.ImageLocation
                    }
                })
                .ToList();

            return Ok(posts);
        }
        catch
        {
            return StatusCode(500, "An error occurred while retrieving your posts.");
        }
    }

    [HttpGet("search")]
    public IActionResult SearchPosts([FromQuery] string searchTerm, [FromQuery] int? categoryId)
    {
        try 
        {
            var query = _dbContext.Posts
                .Include(p => p.UserProfile)
                    .ThenInclude(up => up.IdentityUser)
                .Include(p => p.Category)
                .Where(p => p.IsApproved);

            // Apply category filter
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            // Apply text search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p => 
                    p.Title.ToLower().Contains(searchTerm) ||
                    p.Category.Name.ToLower().Contains(searchTerm)
                );
            }

            List<PostDTO> posts = query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    HeaderImage = p.HeaderImage,
                    IsApproved = p.IsApproved,
                    CategoryName = p.Category.Name,
                    Author = new UserProfileDTO
                    {
                        Id = p.UserProfile.Id,
                        FirstName = p.UserProfile.FirstName,
                        LastName = p.UserProfile.LastName,
                        Email = p.UserProfile.IdentityUser.Email,
                        UserName = p.UserProfile.IdentityUser.UserName,
                        ImageLocation = p.UserProfile.ImageLocation
                    }
                })
                .ToList();

            return Ok(posts);
        }
        catch
        {
            return StatusCode(500, "An error occurred while searching posts.");
        }
    }
} 