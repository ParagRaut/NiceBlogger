namespace NiceBlogger.UseCases.AuthorUseCases.Entities;

public class Author
{
    public Author(AuthorId id, string name, string surname)
    {
        Id = id;
        Name = name;
        Surname = surname;
    }

    public AuthorId Id { get; private set; }

    public string Name { get; private set; }

    public string Surname { get; private set; }

    private Author()
    {

    }
}

public record AuthorId(Guid Value);
