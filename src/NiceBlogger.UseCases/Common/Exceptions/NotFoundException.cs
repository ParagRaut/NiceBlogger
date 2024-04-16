namespace NiceBlogger.UseCases.Common.Exceptions;

public abstract class NotFoundException(string message) : ApplicationException("Not Found", message);