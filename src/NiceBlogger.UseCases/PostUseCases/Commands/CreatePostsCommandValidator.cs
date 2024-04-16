using FluentValidation;

namespace NiceBlogger.UseCases.PostUseCases.Commands;

public class CreatePostsCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostsCommandValidator()
    {
        RuleFor(command => command.AuthorId).NotNull();
        RuleFor(command => command.Title).NotEmpty().MinimumLength(8).MaximumLength(80);
        RuleFor(command => command.Description).NotEmpty().MinimumLength(20);
        RuleFor(command => command.Content).NotEmpty().MinimumLength(20);
    }
}
