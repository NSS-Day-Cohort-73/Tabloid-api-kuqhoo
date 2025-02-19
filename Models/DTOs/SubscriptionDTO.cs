namespace Tabloid.Models.DTOs;

public class SubscriptionDTO
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public DateTime SubscribedAt { get; set; }
    public UserProfileDTO Author { get; set; }
} 