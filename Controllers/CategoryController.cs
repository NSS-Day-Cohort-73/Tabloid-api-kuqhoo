using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabloid.Controllers;
using Tabloid.Data;
using Tabloid.Models;
using Tabloid.Models.DTOs;

[ApiController]
// [Authorize]
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

    [HttpPost]
    public IActionResult CategoryPost(Category category)
    {
        _dbContext.Categories.Add(category);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(int id)
    {
        Category categoryToDelete = _dbContext.Categories.Where(c => c.Id == id).FirstOrDefault();
        _dbContext.Categories.Remove(categoryToDelete);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategory(Category newCategory, int id)
    {
        Category categoryToUpdate = _dbContext.Categories.Where(c => c.Id == id).FirstOrDefault();
        categoryToUpdate.Name = newCategory.Name;
        _dbContext.SaveChanges();
        return NoContent();
    }
}
