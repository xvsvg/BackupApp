namespace Backups.Exceptions;

public class AbstractRepositoryException : BackupExceptions
{
    private AbstractRepositoryException(string message)
        : base(message) { }

    public static AbstractRepositoryException InvalidRepositoryPathException(string path)
        => new AbstractRepositoryException($"Path '{path}' is invalid");
}
