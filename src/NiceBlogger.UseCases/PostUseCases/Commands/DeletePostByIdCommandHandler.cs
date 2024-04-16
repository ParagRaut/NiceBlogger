using MediatR;
using NiceBlogger.UseCases.PostUseCases.Exceptions;
using NiceBlogger.UseCases.PostUseCases.Repositories;

namespace NiceBlogger.UseCases.PostUseCases.Commands;

public class DeletePostByIdCommandHandler(IPostRepository postRepository) : IRequestHandler<DeletePostByIdCommand>
{
    public async Task Handle(DeletePostByIdCommand request, CancellationToken cancellationToken)
    {
        var post = await postRepository.GetByIdAsync(request.Id, cancellationToken);

        if (post is null)
        {
            throw new PostNotFoundException(request.Id.Value);
        }

        await postRepository.DeletePostAsync(post, cancellationToken);
    }
}
