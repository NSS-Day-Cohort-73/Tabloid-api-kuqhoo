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
public class CommentController : ControllerBase
{
    private TabloidDbContext _dbContext;

    public CommentController(TabloidDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet("{postId}")]
    public IActionResult GetCommentsByPostId(int postId)
    {
        try 
        {
            List<CommentDTO> comments = _dbContext.Comments
                .Include(c => c.UserProfile)
                    .ThenInclude(up => up.IdentityUser)
                .Include(c => c.Post)
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentDTO
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    PostId = c.PostId,
                    Author = new UserProfileDTO
                    {
                        Id = c.UserProfile.Id,
                        FirstName = c.UserProfile.FirstName,
                        LastName = c.UserProfile.LastName,
                        Email = c.UserProfile.Email,
                        UserName = c.UserProfile.UserName,
                        ImageLocation = c.UserProfile.ImageLocation
                    }
                })
                .ToList();
            if(comments == null)
            {
                return NotFound();
            }
            return Ok(comments);
        }
        catch
        {
            return StatusCode(500, "An error occurred while retrieving posts.");
        }
    }
};