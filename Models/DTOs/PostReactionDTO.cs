namespace Tabloid.Models.DTOs;

public class PostReactionDTO
{
    public int Id { get; set; }

    public int UserProfileId { get; set; }

    public int PostId { get; set; }

    public int ReactionTypeId { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public ReactionTypeDTO ReactionType { get; set; }
    public UserProfileDTO UserProfile { get; set; }
    public PostDTO Post { get; set; }
}
