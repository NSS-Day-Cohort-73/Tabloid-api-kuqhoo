using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tabloid.Models;
using Tabloid.Models.DTOs;
using Tabloid.Data;
using Microsoft.AspNetCore.Authorization;

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
        if (postToDelete == null)
        {
            return NotFound($"Post with Id = {id} not found");
        }
        _dbContext.Posts.Remove(postToDelete);
        _dbContext.SaveChanges();
        return NoContent();

    }

    [HttpPut("{id}")]
    public IActionResult EditPost(int id, Post post)
    {
        try
        {
            if (id != post.Id)
            {
                return BadRequest("PostId mismatch");
            }
            Post postToUpdate = _dbContext.Posts
                .SingleOrDefault((p) => p.Id == id);
            if (postToUpdate == null)
            {
                return NotFound($"Post with Id = {id} not found");
            }
            postToUpdate.Title = post.Title;
            postToUpdate.Content = post.Content;
            postToUpdate.Category = post.Category;
            _dbContext.SaveChanges();
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating data");
        }
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
} 