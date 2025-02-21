using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tabloid.Data;
using Tabloid.Models;

namespace Tabloid.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private TabloidDbContext _dbContext;

    public TagController(TabloidDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet("list")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetTagList()
    {
        var tagList = _dbContext
            .Tags.Select(t => new { t.Id, t.Name })
            .OrderBy(t => t.Name)
            .ToList();

        return Ok(tagList);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateTag([FromBody] Tag tag)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _dbContext.Tags.Add(tag);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetTagList), new { id = tag.Id }, tag);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateTag(int id, [FromBody] Tag tag)
    {
        if (id != tag.Id)
        {
            return BadRequest();
        }

        var existingTag = _dbContext.Tags.Find(id);
        if (existingTag == null)
        {
            return NotFound();
        }

        existingTag.Name = tag.Name;
        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteTag(int id)
    {
        var tag = _dbContext.Tags.Find(id);
        if (tag == null)
        {
            return NotFound();
        }

        _dbContext.Tags.Remove(tag);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
