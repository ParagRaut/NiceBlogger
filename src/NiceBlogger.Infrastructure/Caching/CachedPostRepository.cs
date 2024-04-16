using Microsoft.Extensions.Caching.Distributed;
using NiceBlogger.Infrastructure.Extensions;
using NiceBlogger.UseCases.PostUseCases.Entities;
using NiceBlogger.UseCases.PostUseCases.Repositories;

namespace NiceBlogger.Infrastructure.Caching;

public class CachedPostRepository : IPostRepository
{
    private readonly IPostRepository _decorated;
    private readonly IDistributedCache _cache;

    public CachedPostRepository(IPostRepository postRepository, IDistributedCache cache)
    {
        _decorated = postRepository;
        _cache = cache;
    }

    public async Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken)
    {
        var cacheKey = $"post-{id}";

        var post = await _cache.GetAsync(
            cacheKey,
            async token => await _decorated.GetByIdAsync(id, token),
            CacheOptions.DefaultExpiration,
            cancellationToken);

        return post;
    }

    public async Task<PostId> CreatePostAsync(Post post, CancellationToken cancellationToken)
    {
        var postId = await _decorated.CreatePostAsync(post, cancellationToken);
        
        var cacheKey = "posts-all";

        await _cache.RemoveAsync(cacheKey, cancellationToken);

        return postId;
    }

    public async Task DeletePostAsync(Post post, CancellationToken cancellationToken)
    {
        await _decorated.DeletePostAsync(post, cancellationToken);
        
        var cacheKey = "posts-all";

        await _cache.RemoveAsync(cacheKey, cancellationToken);
    }

    public async Task<List<Post>> GetAllPostsAsync(CancellationToken cancellationToken, int page, int pageSize)
    {
        var cacheKey = (page == 1 && pageSize == 10) ? "posts-all" : $"posts-all-{page}-{pageSize}";

        var posts = await _cache.GetAsync(
            cacheKey,
            async token => await _decorated.GetAllPostsAsync(token, page, pageSize),
            CacheOptions.DefaultExpiration,
            cancellationToken);

        return posts;
    }
}
