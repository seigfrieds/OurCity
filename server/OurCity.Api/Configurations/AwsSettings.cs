namespace OurCity.Api.Configurations;
public class AwsSettings
{
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; } 
    public required string BucketName { get; set; }
    public required string Region { get; set; }
}