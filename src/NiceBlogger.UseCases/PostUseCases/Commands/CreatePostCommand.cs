using MediatR;
using NiceBlogger.UseCases.AuthorUseCases.Entities;
using NiceBlogger.UseCases.PostUseCases.Entities;

namespace NiceBlogger.UseCases.PostUseCases.Commands;

public record CreatePostCommand(AuthorId AuthorId, string Title, string Description, string Content) : IRequest<PostId>;