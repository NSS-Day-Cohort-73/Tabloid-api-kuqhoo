using System.ComponentModel.DataAnnotations;

namespace Tabloid.Models;

public class Comment
{
    public int Id { get; set; }

    public int UserProfileId { get; set; }

    public int PostId { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public UserProfile UserProfile { get; set; }
    public Post Post { get; set; }
}
