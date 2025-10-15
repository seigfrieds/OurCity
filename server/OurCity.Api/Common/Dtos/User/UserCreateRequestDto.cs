using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Common.Dtos.User;

public class UserCreateRequestDto
{
    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
    public required string Username { get; set; }
    
    [StringLength(100, ErrorMessage = "Display name cannot exceed 100 characters.")]
    public string? DisplayName { get; set; }
}
