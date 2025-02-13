namespace Tabloid.Models.DTOs;

public class PostReactionDTO
{
    public int Id { get; set; }

    public int UserProfileId { get; set; }

    public int PostId { get; set; }

    public int reactionTypeId { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public UserProfileDTO UserProfile { get; set; }
    public PostDTO Post { get; set; }
}
