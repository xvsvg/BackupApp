namespace Backups.Exceptions;

public class BackupTaskException : BackupExceptions
{
    private BackupTaskException(string name)
        : base(name) { }

    public static BackupTaskException InvalidBackupNameException(string name)
        => new BackupTaskException($"Name '{name}' is invalid name for backup task");
}
