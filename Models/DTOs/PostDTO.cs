namespace Tabloid.Models.DTOs;

public class PostDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string HeaderImage { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsApproved { get; set; }
    public string CategoryName { get; set; }
    public UserProfileDTO Author { get; set; }
} 