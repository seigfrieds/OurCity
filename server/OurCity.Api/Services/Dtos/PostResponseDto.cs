using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Services.Dtos;

public class PostResponseDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }
}
