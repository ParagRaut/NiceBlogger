using NiceBlogger.Infrastructure.Data;
using NiceBlogger.UseCases.AuthorUseCases.Entities;
using NiceBlogger.UseCases.AuthorUseCases.Repositories;

namespace NiceBlogger.Infrastructure.Repository;

public class AuthorRepository(ApplicationDbContext context) : IAuthorRepository
{
    public async Task<Author?> GetByIdAsync(AuthorId id, CancellationToken cancellationToken)
    {
        return await context.Authors.FindAsync(id, cancellationToken);
    }
}