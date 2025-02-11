namespace Tabloid.Models.DTOs;

public class SubscriptionDTO
{
    public int Id { get; set; }
    public UserProfileDTO Subscriber { get; set; }
    public UserProfileDTO Author { get; set; }
    public DateTime SubscribedAt { get; set; }
} 