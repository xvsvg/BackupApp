using Backups.Contracts;
using Backups.Entities;
using Backups.Infrastructure.Implementaions.Repositories;

namespace Backups.UI.Contracts;

public interface IBackupProvider
{
    AbstractRepository InitializeFileSystemRepository(PhysicalFileSystemRepository fileSystemRepository);

    BackupTask CreateTask(BackupTask task);
}
