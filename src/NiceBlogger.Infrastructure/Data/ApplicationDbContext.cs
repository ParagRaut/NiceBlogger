using Microsoft.EntityFrameworkCore;
using NiceBlogger.UseCases.AuthorUseCases.Entities;
using NiceBlogger.UseCases.PostUseCases.Entities;

namespace NiceBlogger.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Post> Posts { get; set; }

    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}