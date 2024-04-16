using FluentAssertions;
using NiceBlogger.UseCases.AuthorUseCases.Entities;
using NiceBlogger.UseCases.AuthorUseCases.Exceptions;
using NiceBlogger.UseCases.PostUseCases.Commands;
using NiceBlogger.UseCases.PostUseCases.Queries;
using Posts.IntegrationTests.Abstractions;

namespace Posts.IntegrationTests;

public class CreatePostsTest(IntegrationTestWebAppFactory webAppFactory) : IntegrationTestBase(webAppFactory)
{
    [Fact]
    public async Task CreatePostCommandHandler_Should_CreatePost_WhenAuthor_IsValid()
    {
        // Arrange
        var getAllQuery = new GetAllQuery(1, 1);

        var posts = await Sender.Send(getAllQuery);

        var authorId = posts.FirstOrDefault()!.AuthorId;
        var title = Faker.Lorem.Sentence();
        var description = Faker.Lorem.Paragraph();
        var content = Faker.Lorem.Paragraphs(1);

        // Act
        var command = new CreatePostCommand(authorId, title, description, content);
        var postId = await Sender.Send(command);

        // Assert
        var getByIdQuery = new GetByIdQuery(postId);
        var postResult = await Sender.Send(getByIdQuery);

        postResult.Should().NotBeNull();
        postResult?.AuthorId.Should().Be(authorId);
        postResult?.Title.Should().Be(title);
        postResult?.Description.Should().Be(description);
        postResult?.Content.Should().Be(content);
    }

    [Fact]
    public async Task CreatePostCommandValidator_Should_Throw_ValidationError_Title_ExceedsMaxLength()
    {
        // Arrange
        var authorId = new AuthorId(Guid.NewGuid());
        var title = Faker.Lorem.Paragraph(3);
        var description = Faker.Lorem.Paragraph();
        var content = Faker.Lorem.Paragraphs(1);

        var command = new CreatePostCommand(authorId, title, description, content);

        // Act
        var validationResult = await CreatePostCommandValidator.ValidateAsync(command);

        // Assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
        validationResult.Errors.Should().NotBeNull("Error has occured");
    }

    [Fact]
    public async Task CreatePostCommandValidator_Should_Throw_ValidationError_Description_LessThanMinimumLength()
    {
        // Arrange
        var authorId = new AuthorId(Guid.NewGuid());
        var title = Faker.Lorem.Sentence();
        var description = Faker.Lorem.Letter(4);
        var content = Faker.Lorem.Paragraphs(1);

        var command = new CreatePostCommand(authorId, title, description, content);

        // Act
        var validationResult = await CreatePostCommandValidator.ValidateAsync(command);

        // Assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
        validationResult.Errors.Should().NotBeNull("Error has occured");
        validationResult.Errors.Should().Contain(
            a => a.ErrorMessage.Equals(
                "The length of 'Description' must be at least 20 characters. You entered 4 characters."));
    }

    [Fact]
    public async Task CreatePostCommandValidator_Should_Throw_ValidationError_Content_LessThanMinimumLength()
    {
        // Arrange
        var authorId = new AuthorId(Guid.NewGuid());
        var title = Faker.Lorem.Sentence();
        var description = Faker.Lorem.Paragraph(3);
        var content = Faker.Lorem.Letter(4);

        var command = new CreatePostCommand(authorId, title, description, content);

        // Act
        var validationResult = await CreatePostCommandValidator.ValidateAsync(command);

        // Assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
        validationResult.Errors.Should().NotBeNull("Error has occured");
        validationResult.Errors.Should().Contain(
            a => a.ErrorMessage.Equals(
                "The length of 'Content' must be at least 20 characters. You entered 4 characters."));
    }

    [Fact]
    public async Task CreatePostCommandHandler_Should_Throw_AuthorNotFoundException_WhenAuthor_DoesNotExist()
    {
        // Arrange
        var authorId = new AuthorId(Guid.NewGuid());
        var title = Faker.Lorem.Sentence();
        var description = Faker.Lorem.Paragraph();
        var content = Faker.Lorem.Paragraphs(1);

        // Act
        var command = new CreatePostCommand(authorId, title, description, content);
        var act = async () => await Sender.Send(command);

        // Assert
       await act.Should().ThrowAsync<AuthorNotFoundException>();
    }
}
