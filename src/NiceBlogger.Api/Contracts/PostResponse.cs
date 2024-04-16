namespace NiceBlogger.Api.Contracts;

public class PostResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public AuthorResponse Author { get; set; }
}