using Bogus;
using Microsoft.EntityFrameworkCore;
using NiceBlogger.Infrastructure.Data;
using NiceBlogger.UseCases.AuthorUseCases.Entities;
using NiceBlogger.UseCases.PostUseCases.Entities;

namespace NiceBlogger.Api.Extensions;

public static class LocalDevelopmentExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using var dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }

    public static void SeedData(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using var dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Check if there's already data
        if (dbContext.Authors.Any() || dbContext.Posts.Any())
        {
            return; // Data already seeded
        }

        // Create a Faker generator
        var faker = new Faker("en_IND");

        // Seed authors
        var authors = GenerateAuthors(faker,10);

        // Add authors to the database context and save changes
        dbContext.Authors.AddRange(authors);
        dbContext.SaveChanges();

        // Seed posts
        var posts = GeneratePosts(faker,50, authors);

        // Add posts to the database context and save changes
        dbContext.Posts.AddRange(posts);
        dbContext.SaveChanges();
    }

    private static List<Author> GenerateAuthors(Faker faker, int numberOfAuthors)
    {
        var authors =
            // Generate authors
            Enumerable.Range(1, numberOfAuthors)
            .Select(
                _ => new Author(
                    new AuthorId(faker.Random.Guid()),
                    faker.Name.FirstName(), 
                    faker.Name.LastName()))
            .ToList();

        return authors;
    }

    private static List<Post> GeneratePosts(Faker faker, int numberOfPosts, List<Author> authors)
    {
        var posts =
            // Generate posts
            Enumerable.Range(1, numberOfPosts)
            .Select(_ => 
                new Post(
                    new PostId(faker.Random.Guid()),
                    faker.PickRandom(authors.Select(a => a.Id).ToArray()),
                    faker.Lorem.Sentence(),
                    faker.Lorem.Sentences(2),
                    faker.Rant.Review("NiceBlogger")
                ))
            .ToList();

        return posts;
    }
}
