namespace Backups.Exceptions;

public class BackupExceptions : Exception
{
    public BackupExceptions()
        : base() { }

    public BackupExceptions(string message)
        : base(message) { }

    public BackupExceptions(string message, Exception innerException)
        : base(message, innerException) { }
}
