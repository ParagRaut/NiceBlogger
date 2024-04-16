using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using NiceBlogger.Api.Contracts;
using Posts.FunctionalTests.Abstractions;

namespace Posts.FunctionalTests;

public class PostsApiTests(FunctionalTestWebAppFactory webAppFactory) : FunctionalTestBase(webAppFactory)
{
    [Fact]
    public async Task GetPostByIdReturnsPostWithAuthorInformation()
    {
        // Arrange
        var postAllResponse = await HttpClient.GetAsync($"{BaseUrl}?page=1&pageSize=20");
        
        postAllResponse.EnsureSuccessStatusCode();
        var postAllBody = await postAllResponse.Content.ReadAsStreamAsync();
        var posts = await JsonSerializer.DeserializeAsync<List<PostResponse>>(postAllBody, JsonSerializerOptions);
        var postId = Faker.PickRandom(posts?.Select(p => p.Id));

        // Act
        var postById = await HttpClient.GetAsync($"{BaseUrl}/{postId}?includeAuthor=true");

        postById.EnsureSuccessStatusCode();
        var postByIdBody = await postById.Content.ReadAsStreamAsync();
        var postResponse = await JsonSerializer.DeserializeAsync<PostResponse>(postByIdBody, JsonSerializerOptions);

        // Assert
        postResponse?.Should().NotBeNull();
        postResponse?.Author.Should().NotBeNull();
        postResponse?.Id.Should().Be(postId);
    }

    [Fact]
    public async Task GetPostItemsRespectsPageSize()
    {
        // Act
        var response = await HttpClient.GetAsync($"{BaseUrl}?page=1&pageSize=20");

        // Assert
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<List<PostResponse>>(body, JsonSerializerOptions);
        
        result?.Count.Should().Be(20);
    }

    [Fact]
    public async Task GetPostItemsHasAuthorInformation()
    {
        // Act
        var response = await HttpClient.GetAsync($"{BaseUrl}?includeAuthor=true&page=1&pageSize=20");

        // Assert
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<List<PostResponse>>(body, JsonSerializerOptions);
        
        result?.Count.Should().Be(20);
        result.Should().AllSatisfy(p => p.Author.Should().NotBeNull());
        result.Should().NotContainNulls(p => p.Author);
    }

    [Fact]
    public async Task CreateNewPostWithValidAuthorId()
    {
        // Arrange
        var postAllResponse = await HttpClient.GetAsync($"{BaseUrl}?includeAuthor=true&page=1&pageSize=20");
        
        postAllResponse.EnsureSuccessStatusCode();
        var postAllBody = await postAllResponse.Content.ReadAsStreamAsync();
        var posts = await JsonSerializer.DeserializeAsync<List<PostResponse>>(postAllBody, JsonSerializerOptions);
        var authorId = Faker.PickRandom(posts?.Select(p => p.Author.Id));

        var request = new CreateNewPostRequest
        {
            AuthorId = authorId,
            Title = Faker.Lorem.Sentence(),
            Description = Faker.Lorem.Paragraph(),
            Content = Faker.Lorem.Paragraphs(1)
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // Act
        var response = await HttpClient.PostAsync("api/v1/post", content);
        var postId = await response.Content.ReadAsStringAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.Should().HaveStatusCode(HttpStatusCode.Created);
        postId.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreateNewPostWithInvalidAuthorId()
    {
        // Arrange
        var authorId = Guid.NewGuid();

        var request = new CreateNewPostRequest
        {
            AuthorId = authorId,
            Title = Faker.Lorem.Sentence(),
            Description = Faker.Lorem.Paragraph(),
            Content = Faker.Lorem.Paragraphs(1)
        };

        // Act
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await HttpClient.PostAsync("api/v1/post", content);
        
        // Assert
        response.Should().HaveClientError();
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
        response.Should().NotHaveStatusCode(HttpStatusCode.Created);
    }

    [Fact]
    public async Task GetPostByIdReturnsPostWithoutAuthorInformation()
    {
        // Arrange
        var postAllResponse = await HttpClient.GetAsync($"{BaseUrl}?page=1&pageSize=20");
        
        postAllResponse.EnsureSuccessStatusCode();
        var postAllBody = await postAllResponse.Content.ReadAsStreamAsync();
        var posts = await JsonSerializer.DeserializeAsync<List<PostResponse>>(postAllBody, JsonSerializerOptions);
        var postId = Faker.PickRandom(posts?.Select(p => p.Id));

        // Act
        var postById = await HttpClient.GetAsync($"{BaseUrl}/{postId}?includeAuthor=false");

        postById.EnsureSuccessStatusCode();
        var postByIdBody = await postById.Content.ReadAsStreamAsync();
        var postResponse = await JsonSerializer.DeserializeAsync<PostResponse>(postByIdBody, JsonSerializerOptions);

        // Assert
        postResponse?.Should().NotBeNull();
        postResponse?.Id.Should().Be(postId);
    }
}
