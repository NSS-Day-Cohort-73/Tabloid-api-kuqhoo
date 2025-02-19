using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tabloid.Data;
using Tabloid.Models;
using Tabloid.Models.DTOs;

namespace Tabloid.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class ReactionController : ControllerBase
{
    private TabloidDbContext _dbContext;

    public ReactionController(TabloidDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet("types")]
    public IActionResult GetReactionTypes()
    {
        List<ReactionTypeDTO> reactionTypes = _dbContext
            .ReactionTypes.Select(rt => new ReactionTypeDTO
            {
                Id = rt.Id,
                Type = rt.Type,
                FaIcon = rt.FaIcon,
            })
            .ToList();

        return Ok(reactionTypes);
    }

    [HttpGet("postreactions")]
    public IActionResult GetPostReactions()
    {
        List<PostReactionDTO> postReactions = _dbContext
            .PostReactions.Include(pr => pr.UserProfile)
            .Include(pr => pr.Post)
            .ThenInclude(p => p.Category)
            .Include(pr => pr.Post)
            .ThenInclude(p => p.UserProfile)
            .Select(pr => new PostReactionDTO
            {
                Id = pr.Id,
                UserProfileId = pr.UserProfileId,
                PostId = pr.PostId,
                ReactionTypeId = pr.ReactionTypeId,
                CreatedAt = pr.CreatedAt,
                ReactionType = _dbContext
                    .ReactionTypes.Where(rt => rt.Id == pr.ReactionTypeId)
                    .Select(rt => new ReactionTypeDTO
                    {
                        Id = rt.Id,
                        Type = rt.Type,
                        FaIcon = rt.FaIcon,
                    })
                    .FirstOrDefault(),
                UserProfile = new UserProfileDTO
                {
                    Id = pr.UserProfile.Id,
                    FirstName = pr.UserProfile.FirstName,
                    LastName = pr.UserProfile.LastName,
                    ImageLocation = pr.UserProfile.ImageLocation,
                },
                Post = new PostDTO
                {
                    Id = pr.Post.Id,
                    Title = pr.Post.Title,
                    Content = pr.Post.Content,
                    HeaderImage = pr.Post.HeaderImage,
                    CreatedAt = pr.Post.CreatedAt,
                    IsApproved = pr.Post.IsApproved,
                    CategoryName = pr.Post.Category.Name,
                    Author = new UserProfileDTO
                    {
                        Id = pr.Post.UserProfile.Id,
                        FirstName = pr.Post.UserProfile.FirstName,
                        LastName = pr.Post.UserProfile.LastName,
                        ImageLocation = pr.Post.UserProfile.ImageLocation,
                    },
                },
            })
            .ToList();

        return Ok(postReactions);
    }

    [HttpPost("type")]
    public IActionResult PostAType(ReactionType newReactionType)
    {
        _dbContext.ReactionTypes.Add(newReactionType);
        _dbContext.SaveChanges();
        return NoContent();
    }
}
