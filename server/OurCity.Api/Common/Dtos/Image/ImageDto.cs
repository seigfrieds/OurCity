using System.ComponentModel.DataAnnotations;

namespace OurCity.Api.Common.Dtos.Image;

public class ImageDto
{
    [Required(ErrorMessage = "Url is required")]
    public required string Url { get; set; }
}
