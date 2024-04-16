using FluentAssertions;
using NiceBlogger.UseCases.PostUseCases.Entities;
using NiceBlogger.UseCases.PostUseCases.Queries;
using Posts.IntegrationTests.Abstractions;

namespace Posts.IntegrationTests;

public class GetPostsTest(IntegrationTestWebAppFactory webAppFactory) : IntegrationTestBase(webAppFactory)
{
    [Fact]
    public async Task GetPostByIdQueryHandler_Should_ReturnPost()
    {
        // Arrange
        var getAllQuery = new GetAllQuery(1, 20);

        var posts = await Sender.Send(getAllQuery);

        var postId = Faker.PickRandom(posts.Select(p => p.Id));

        // Act
        var getByIdQuery = new GetByIdQuery(postId);
        var postResult = await Sender.Send(getByIdQuery);

        // Assert
        postResult.Should().NotBeNull();
        postResult?.Should().BeOfType<Post?>();
    }
    
    [Fact]
    public async Task GetAllPostQueryHandler_Should_Return_CorrectPostCount_RespectingPageSize()
    {
        // Arrange
        var getAllQuery = new GetAllQuery(1, 20);

        // Act
        var posts = await Sender.Send(getAllQuery);

        // Assert
        posts.Should().NotBeNull();
        posts.Should().BeOfType<List<Post?>>();

        posts.Count.Should().Be(20);
    }
}