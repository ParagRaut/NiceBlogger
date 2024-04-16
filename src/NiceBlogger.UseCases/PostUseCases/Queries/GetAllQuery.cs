using MediatR;
using NiceBlogger.UseCases.PostUseCases.Entities;

namespace NiceBlogger.UseCases.PostUseCases.Queries;

public record GetAllQuery(int Page = 1, int PageSize = 10) : IRequest<List<Post>>;