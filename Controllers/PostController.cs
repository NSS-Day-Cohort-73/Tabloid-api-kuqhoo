using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tabloid.Models;
using Tabloid.Models.DTOs;
using Tabloid.Data;

namespace Tabloid.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        return Ok(_dbContext.Posts
            .Include(p => p.UserProfile)
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
                    UserName = p.UserProfile.UserName,
                    Email = p.UserProfile.Email,
                    ImageLocation = p.UserProfile.ImageLocation
                }
            }));
    }
} 