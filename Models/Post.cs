using System.ComponentModel.DataAnnotations;

namespace Tabloid.Models;

public class Post
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    public int CategoryId { get; set; }
    
    public bool IsApproved { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [DataType(DataType.Url)]
    public string HeaderImage { get; set; }
    
    // Navigation properties
    public UserProfile UserProfile { get; set; }
    public Category Category { get; set; }
} 