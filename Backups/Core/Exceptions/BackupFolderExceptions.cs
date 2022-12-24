using Backups.Exceptions;

namespace Backups.Core.Exceptions;

public class BackupFolderExceptions : BackupExceptions
{
    private BackupFolderExceptions(string message)
        : base(message) { }

    public static BackupFolderExceptions NotSupportPathException(string message)
        => new BackupFolderExceptions(message);
}
