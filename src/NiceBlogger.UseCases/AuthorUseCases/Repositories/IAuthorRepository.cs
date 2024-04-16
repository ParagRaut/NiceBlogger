using NiceBlogger.UseCases.AuthorUseCases.Entities;

namespace NiceBlogger.UseCases.AuthorUseCases.Repositories;

public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(AuthorId id, CancellationToken cancellationToken);
}