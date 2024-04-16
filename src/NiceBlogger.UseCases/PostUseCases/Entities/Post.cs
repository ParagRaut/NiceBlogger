using NiceBlogger.UseCases.AuthorUseCases.Entities;

namespace NiceBlogger.UseCases.PostUseCases.Entities;

//Can be an auditable entity but not using it to keep things simple 
public class Post
{
    public Post(
        PostId id,
        AuthorId authorId,
        string title,
        string description,
        string content)
    {
        Id = id;
        AuthorId = authorId;
        Title = title;
        Description = description;
        Content = content;
    }

    public PostId Id { get; private set; }

    public AuthorId AuthorId { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Content { get; private set; }

    private Post()
    {
    }
}

public record PostId(Guid Value);
