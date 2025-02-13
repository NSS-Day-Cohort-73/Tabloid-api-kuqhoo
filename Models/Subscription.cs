using System.ComponentModel.DataAnnotations;

namespace Tabloid.Models;

public class Subscription
{
    public int Id { get; set; }

    public int UserProfileId { get; set; }

    public int AuthorId { get; set; }

    public DateTime SubscribedAt { get; set; }

    // Navigation properties
    public UserProfile Subscriber { get; set; }
    public UserProfile Author { get; set; }
}
