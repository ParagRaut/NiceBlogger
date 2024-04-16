using MediatR;
using NiceBlogger.UseCases.PostUseCases.Entities;

namespace NiceBlogger.UseCases.PostUseCases.Commands;

public record DeletePostByIdCommand(PostId Id) : IRequest;
