using Backups.Exceptions;

namespace Backups.Core.Exceptions;

public class RepositoryExceptions : BackupExceptions
{
    private RepositoryExceptions(string message)
        : base(message) { }

    public static RepositoryExceptions NotSupportedPathException(string message)
        => new RepositoryExceptions(message);
}
