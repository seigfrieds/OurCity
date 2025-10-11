using Microsoft.EntityFrameworkCore;
using OurCity.Api.Infrastructure.Database;

try
{
    Console.WriteLine("Migrating...");

    //configure dbcontext
    string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
    if (connectionString is null)
        throw new Exception("Connection string required as environment variable");

    var options = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString).Options;

    //run migrations
    using var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    dbContext.Database.Migrate();

    Console.WriteLine("Migration complete");
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine($"Migration failed: {ex.Message}");
    return 1;
}
