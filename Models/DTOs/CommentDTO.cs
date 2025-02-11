namespace Tabloid.Models.DTOs;

public class CommentDTO
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserProfileDTO Author { get; set; }
    public int PostId { get; set; }
} 