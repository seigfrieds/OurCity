using Microsoft.EntityFrameworkCore;

namespace OurCity.Api.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    //OnModelCreating -> Can get more granular about what the tables will look like
    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("posts");
            entity.HasKey(e => e.PostId);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(100);
        });
    }*/
}
