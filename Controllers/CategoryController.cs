using Microsoft.AspNetCore.Mvc;
using Tabloid.Controllers;
using Tabloid.Data;
using Tabloid.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private TabloidDbContext _dbContext;

    public CategoryController(TabloidDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    public IActionResult CategoryGet()
    {
        return Ok(
            _dbContext
                .Categories.OrderBy(c => c.Id)
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    // PostCount = c.PostCount, // can add later if we need the post count after it is updated in the database (or turned into a calculated property)
                })
        );
    }
}
