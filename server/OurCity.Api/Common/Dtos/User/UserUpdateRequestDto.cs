using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Common.Dtos.User;

public class UserUpdateRequestDto
{
    [Required(ErrorMessage = "Id is required")]
    public required int Id { get; set; }

    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
    public string? Username { get; set; } = null;

    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; } = null;

    [StringLength(100, ErrorMessage = "Display name cannot exceed 100 characters.")]
    public string? DisplayName { get; set; } = null;
}
