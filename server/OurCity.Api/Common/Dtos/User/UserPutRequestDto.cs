using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Common.Dtos.User;

public class UserPutRequestDto
{
    [Required(ErrorMessage = "id is required")]
    public required string id { get; set; }

    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
    public string? Username { get; set; } = null;

    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; } = null;

    [StringLength(100, ErrorMessage = "Display name cannot exceed 100 characters.")]
    public string? DisplayName { get; set; } = null;
}
