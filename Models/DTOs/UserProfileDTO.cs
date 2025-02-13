using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Tabloid.Models.DTOs;

public class UserProfileDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [NotMapped]
    public string Email { get; set; }

    [NotMapped]
    public string UserName { get; set; }
    public IdentityUser IdentityUser { get; set; }
    public string ImageLocation { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}
