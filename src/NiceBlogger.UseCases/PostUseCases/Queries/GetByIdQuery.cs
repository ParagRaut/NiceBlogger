using MediatR;
using NiceBlogger.UseCases.PostUseCases.Entities;

namespace NiceBlogger.UseCases.PostUseCases.Queries;

public record GetByIdQuery(PostId ID) : IRequest<Post?>;