using System.ComponentModel.DataAnnotations;

namespace Tabloid.Models;

public class PostReaction
{
    public int Id { get; set; }

    public int UserProfileId { get; set; }

    public int PostId { get; set; }

    public int ReactionTypeId { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public UserProfile UserProfile { get; set; }
    public Post Post { get; set; }
    public ReactionType ReactionType { get; set; }
}
