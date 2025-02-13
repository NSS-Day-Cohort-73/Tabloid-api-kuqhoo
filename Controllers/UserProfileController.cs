using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tabloid.Data;
using Tabloid.Models;

namespace Tabloid.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private TabloidDbContext _dbContext;

    public UserProfileController(TabloidDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok(_dbContext.UserProfiles.ToList());
    }

    [HttpGet("withroles")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetWithRoles()
    {
        return Ok(
            _dbContext
                .UserProfiles.Include(up => up.IdentityUser)
                .Select(up => new UserProfile
                {
                    Id = up.Id,
                    FirstName = up.FirstName,
                    LastName = up.LastName,
                    Email = up.IdentityUser.Email,
                    UserName = up.IdentityUser.UserName,
                    IdentityUserId = up.IdentityUserId,
                    Roles = _dbContext
                        .UserRoles.Where(ur => ur.UserId == up.IdentityUserId)
                        .Select(ur => _dbContext.Roles.SingleOrDefault(r => r.Id == ur.RoleId).Name)
                        .ToList(),
                })
        );
    }

    [HttpPost("promote/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Promote(string id)
    {
        IdentityRole role = _dbContext.Roles.SingleOrDefault(r => r.Name == "Admin");
        _dbContext.UserRoles.Add(new IdentityUserRole<string> { RoleId = role.Id, UserId = id });
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpPost("demote/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Demote(string id)
    {
        IdentityRole role = _dbContext.Roles.SingleOrDefault(r => r.Name == "Admin");

        IdentityUserRole<string> userRole = _dbContext.UserRoles.SingleOrDefault(ur =>
            ur.RoleId == role.Id && ur.UserId == id
        );

        _dbContext.UserRoles.Remove(userRole);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        UserProfile user = _dbContext
            .UserProfiles.Include(up => up.IdentityUser)
            .SingleOrDefault(up => up.Id == id);

        if (user == null)
        {
            return NotFound();
        }
        user.Email = user.IdentityUser.Email;
        user.UserName = user.IdentityUser.UserName;
        return Ok(user);
    }

    [HttpGet("admin/details/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetUserProfileDetails(int id)
    {
        var userProfile = _dbContext
            .UserProfiles.Include(up => up.IdentityUser)
            .FirstOrDefault(up => up.Id == id);

        if (userProfile == null)
        {
            return NotFound();
        }

        var roles = _dbContext
            .UserRoles.Where(ur => ur.UserId == userProfile.IdentityUserId)
            .Select(ur => _dbContext.Roles.SingleOrDefault(r => r.Id == ur.RoleId).Name)
            .ToList();

        return Ok(
            new
            {
                userProfile.Id,
                userProfile.FullName,
                userProfile.ImageLocation,
                UserName = userProfile.IdentityUser.UserName,
                Email = userProfile.IdentityUser.Email,
                CreateDateTime = userProfile.CreateDateTime,
                UserType = roles.Contains("Admin") ? "Admin" : "Author",
            }
        );
    }

    [HttpGet("admin/list")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAdminUserList()
    {
        var userList = _dbContext
            .UserProfiles.Include(up => up.IdentityUser)
            .Select(up => new
            {
                up.Id,
                up.FullName,
                UserName = up.IdentityUser.UserName,
                Roles = _dbContext
                    .UserRoles.Where(ur => ur.UserId == up.IdentityUserId)
                    .Select(ur => _dbContext.Roles.SingleOrDefault(r => r.Id == ur.RoleId).Name)
                    .ToList(),
            })
            .OrderBy(up => up.UserName)
            .ToList();

        return Ok(userList);
    }
}
