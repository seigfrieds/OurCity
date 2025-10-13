using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Services.Dtos;

public class UserRequestDto
{
    [Required(ErrorMessage = "Auth0Id is required")]
    public required string Auth0Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }

    public string? DisplayName { get; set; }
}
