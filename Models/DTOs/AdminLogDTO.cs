namespace Tabloid.Models.DTOs;

public class AdminLogDTO
{
    public int Id { get; set; }
    public UserProfileDTO Admin { get; set; }
    public bool Approval { get; set; }
    public DateTime CreatedAt { get; set; }
} 