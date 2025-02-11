namespace Tabloid.Models.DTOs;

public class ReactionDTO
{
    public int Id { get; set; }
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserProfileDTO User { get; set; }
    public int PostId { get; set; }
} 