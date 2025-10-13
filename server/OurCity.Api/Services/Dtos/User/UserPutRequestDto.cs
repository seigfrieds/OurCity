using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Services.Dtos;

public class UserPutRequestDto
{
    [Required(ErrorMessage = "id is required")]
    public required string id { get; set; }

    public string? Username { get; set; } = null;

    public string? Email { get; set; } = null;

    public string? DisplayName { get; set; } = null;
}
