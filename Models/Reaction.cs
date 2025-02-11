using System.ComponentModel.DataAnnotations;

namespace Tabloid.Models;

public class Reaction
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int PostId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Type { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public UserProfile UserProfile { get; set; }
    public Post Post { get; set; }
} 