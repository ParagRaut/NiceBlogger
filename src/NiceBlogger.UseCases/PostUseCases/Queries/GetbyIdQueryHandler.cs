using MediatR;
using NiceBlogger.UseCases.PostUseCases.Entities;
using NiceBlogger.UseCases.PostUseCases.Repositories;

namespace NiceBlogger.UseCases.PostUseCases.Queries;

public class GetbyIdQueryHandler(IPostRepository postRepository) : IRequestHandler<GetByIdQuery, Post?>
{
    public async Task<Post?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(request.ID, cancellationToken);

        return post;
    }
}