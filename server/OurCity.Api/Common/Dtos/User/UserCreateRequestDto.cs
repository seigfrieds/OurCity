using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Common.Dtos.User;

public class UserCreateRequestDto
{
    [Required(ErrorMessage = "Auth0Id is required")]
    public required string Auth0Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public required string Email { get; set; }

    [StringLength(100, ErrorMessage = "Display name cannot exceed 100 characters.")]
    public string? DisplayName { get; set; }
}
