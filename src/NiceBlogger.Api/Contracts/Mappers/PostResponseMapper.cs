using NiceBlogger.UseCases.AuthorUseCases.Entities;
using NiceBlogger.UseCases.PostUseCases.Entities;

namespace NiceBlogger.Api.Contracts.Mappers;

public static class PostResponseMapper
{
    public static PostResponse MapToPostResponse(Post post, Author author = null)
    {
        var postResponse = new PostResponse
        {
            Id = post.Id.Value,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            Author = MapToAuthorResponse(author)
        };

        return postResponse;
    }

    private static AuthorResponse MapToAuthorResponse(Author author)
    {
        return author != null
            ? new AuthorResponse
            {
                Id = author.Id.Value,
                Name = author.Name,
                Surname = author.Surname
            }
            : null;
    }
}