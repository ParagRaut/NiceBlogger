using MediatR;
using NiceBlogger.UseCases.AuthorUseCases.Entities;

namespace NiceBlogger.UseCases.AuthorUseCases.Queries;

public record GetAuthorByIdQuery(AuthorId ID) : IRequest<Author?>;