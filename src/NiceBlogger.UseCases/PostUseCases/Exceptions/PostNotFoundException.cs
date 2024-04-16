using NiceBlogger.UseCases.Common.Exceptions;

namespace NiceBlogger.UseCases.PostUseCases.Exceptions;

public class PostNotFoundException(Guid postId) : NotFoundException($"The post with the identifier {postId} was not found.");
