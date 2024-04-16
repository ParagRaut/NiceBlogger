using MediatR;
using NiceBlogger.UseCases.PostUseCases.Entities;
using NiceBlogger.UseCases.PostUseCases.Repositories;

namespace NiceBlogger.UseCases.PostUseCases.Queries;

public class GetAllQueryHandler(IPostRepository postRepository) : IRequestHandler<GetAllQuery, List<Post>>
{
    public async Task<List<Post>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var posts = await postRepository.GetAllPostsAsync(
            cancellationToken,
            request.Page,
            request.PageSize);

        return posts;
    }
}
