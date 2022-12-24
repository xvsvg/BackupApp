using Backups.Entities;

namespace Backups.Contracts;

public interface IBackupTask
{
    RestorePoint CreateRestorePointAndStorage(string storageName);
}
