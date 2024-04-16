namespace NiceBlogger.Api.Contracts;

public class CreateNewPostRequest
{
    public Guid AuthorId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
}
