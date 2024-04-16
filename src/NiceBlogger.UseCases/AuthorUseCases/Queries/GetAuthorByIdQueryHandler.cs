using MediatR;
using NiceBlogger.UseCases.AuthorUseCases.Entities;
using NiceBlogger.UseCases.AuthorUseCases.Repositories;

namespace NiceBlogger.UseCases.AuthorUseCases.Queries;

public class GetAuthorByIdQueryHandler(IAuthorRepository authorRepository) : IRequestHandler<GetAuthorByIdQuery, Author?>
{
    public async Task<Author?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetByIdAsync(request.ID, cancellationToken);

        return author;
    }
}