using Backups.Contracts;
using Backups.Entities;
using Backups.Infrastructure.Implementaions.Repositories;
using Backups.UI.Contracts;

namespace Backups.UI;

public class BackupProvider : IBackupProvider
{
    private readonly List<BackupTask> _backupTasks;
    private readonly List<AbstractRepository> _repositories;

    public BackupProvider()
    {
        _backupTasks = new List<BackupTask>();
        _repositories = new List<AbstractRepository>();
    }

    public BackupTask CreateTask(BackupTask task)
    {
        ArgumentNullException.ThrowIfNull(task);
        _backupTasks.Add(task);

        return task;
    }

    public AbstractRepository InitializeFileSystemRepository(PhysicalFileSystemRepository fileSystemRepository)
    {
        ArgumentNullException.ThrowIfNull(fileSystemRepository);

        _repositories.Add(fileSystemRepository);

        return fileSystemRepository;
    }

    public AbstractRepository InitializeInMemoryRepository(InMemoryRepository inMemoryRepository)
    {
        ArgumentNullException.ThrowIfNull(inMemoryRepository);

        _repositories.Add(inMemoryRepository);

        return inMemoryRepository;
    }
}
