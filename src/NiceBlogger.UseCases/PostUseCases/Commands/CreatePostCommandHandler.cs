using MediatR;
using NiceBlogger.UseCases.AuthorUseCases.Exceptions;
using NiceBlogger.UseCases.AuthorUseCases.Repositories;
using NiceBlogger.UseCases.PostUseCases.Entities;
using NiceBlogger.UseCases.PostUseCases.Repositories;

namespace NiceBlogger.UseCases.PostUseCases.Commands;

public class CreatePostCommandHandler(IPostRepository postRepository, IAuthorRepository authorRepository)
    : IRequestHandler<CreatePostCommand, PostId>
{
    public async Task<PostId> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetByIdAsync(request.AuthorId, cancellationToken);

        if (author is null)
        {
            throw new AuthorNotFoundException(request.AuthorId.Value);
        }

        var post = new Post(
            new PostId(Guid.NewGuid()), 
            request.AuthorId, 
            request.Title, 
            request.Description,
            request.Content);

        await postRepository.CreatePostAsync(post, cancellationToken);

        return post.Id;
    }
}
