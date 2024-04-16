using NiceBlogger.UseCases.PostUseCases.Entities;

namespace NiceBlogger.UseCases.PostUseCases.Repositories;

// Separate read/write repository can be created to further enhance CQRS
public interface IPostRepository
{
    Task<List<Post>> GetAllPostsAsync(CancellationToken cancellationToken, int page, int pageSize);

    Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken);

    Task<PostId> CreatePostAsync(Post post, CancellationToken cancellationToken);

    Task DeletePostAsync(Post post, CancellationToken cancellationToken);
}
