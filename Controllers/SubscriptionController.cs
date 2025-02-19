using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using Tabloid.Data;
using Tabloid.Models;
using Tabloid.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Tabloid.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private TabloidDbContext _dbContext;

        public SubscriptionController(TabloidDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost]
        public IActionResult Subscribe([FromBody] SubscriptionCreateDTO subscriptionDto)
        {
            try
            {
                string identityUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                UserProfile subscriber = _dbContext.UserProfiles
                    .SingleOrDefault(up => up.IdentityUserId == identityUserId);

                if (subscriber == null)
                {
                    return NotFound("Subscriber Not Found");
                }

                Subscription subscription = new()
                {
                    UserProfileId = subscriber.Id,
                    AuthorId = subscriptionDto.AuthorId,
                    SubscribedAt = DateTime.Now
                };

                _dbContext.Subscriptions.Add(subscription);
                _dbContext.SaveChanges();

                return Created($"/api/subscription/{subscription.Id}", subscription);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to create subscription");
            }
        }

        [HttpGet("my")]
        public IActionResult GetMySubscriptions()
        {
            string identityUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            List<SubscriptionDTO> subscriptions = _dbContext.Subscriptions
                .Include(s => s.Author)
                    .ThenInclude(a => a.IdentityUser)
                .Include(s => s.Subscriber)
                .Where(s => s.Subscriber.IdentityUserId == identityUserId)
                .Select(s => new SubscriptionDTO
                {
                    Id = s.Id,
                    AuthorId = s.AuthorId,
                    SubscribedAt = s.SubscribedAt,
                    Author = new UserProfileDTO
                    {
                        Id = s.Author.Id,
                        FirstName = s.Author.FirstName,
                        LastName = s.Author.LastName,
                        UserName = s.Author.IdentityUser.UserName,
                        ImageLocation = s.Author.ImageLocation
                    }
                })
                .ToList();

            return Ok(subscriptions);
        }

        [HttpGet("check/{authorId}")]
        public IActionResult CheckSubscription(int authorId)
        {
            string identityUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            bool isSubscribed = _dbContext.Subscriptions
                .Include(s => s.Subscriber)
                .Any(s => s.AuthorId == authorId && 
                          s.Subscriber.IdentityUserId == identityUserId);

            return Ok(isSubscribed);
        }

        [HttpDelete("{authorId}")]
        public IActionResult Unsubscribe(int authorId)
        {
            try
            {
                string identityUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                Subscription subscription = _dbContext.Subscriptions
                    .Include(s => s.Subscriber)
                    .SingleOrDefault(s => s.AuthorId == authorId && 
                                        s.Subscriber.IdentityUserId == identityUserId);

                if (subscription == null)
                {
                    return NotFound("Subscription not found");
                }

                _dbContext.Subscriptions.Remove(subscription);
                _dbContext.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to unsubscribe");
            }
        }
    }
} 