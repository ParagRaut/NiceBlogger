namespace NiceBlogger.UseCases.Common.Exceptions;

public sealed class ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary) : ApplicationException(
    "Validation Failure",
    "One or more validation errors occurred")
{
    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; } = errorsDictionary;
}