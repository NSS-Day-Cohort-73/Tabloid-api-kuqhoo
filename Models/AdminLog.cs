using System.ComponentModel.DataAnnotations;

namespace Tabloid.Models;

public class AdminLog
{
    public int Id { get; set; }
    
    public int AdminId { get; set; }
    
    [Required]
    public bool Approval { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    // Navigation property
    public UserProfile Admin { get; set; }
} 