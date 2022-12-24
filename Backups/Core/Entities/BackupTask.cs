using Backups.Contracts;
using Backups.Core.Contracts;
using Backups.Exceptions;

namespace Backups.Entities;

public class BackupTask : IBackupTask
{
    private readonly AbstractRepository _repository;
    private readonly IStorageAlgorithm _strategy;
    private readonly Backup _backup;
    private readonly List<IStorage> _storages;
    private List<ITrackableElement> _files;

    public BackupTask(string name, AbstractRepository repository, IStorageAlgorithm saveStrategy)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(saveStrategy);
        if (string.IsNullOrEmpty(name))
            throw BackupTaskException.InvalidBackupNameException(name);

        _repository = repository;
        _strategy = saveStrategy;
        TaskName = name;
        _files = new List<ITrackableElement>();
        _backup = new Backup();
        _storages = new List<IStorage>();
    }

    public string TaskName { get; private set; }
    public IEnumerable<ITrackableElement> TrackingFiles => _files;
    public Backup Backup => _backup;
    public IEnumerable<IStorage> Storages => _storages;

    public RestorePoint CreateRestorePointAndStorage(string storageName)
    {
        ArgumentNullException.ThrowIfNull(storageName);

        IEnumerable<IStorage> storages = _strategy.CreateStorage(_files, _repository, storageName);

        _storages.AddRange(storages);
        var restorePoint = new RestorePoint(storages, _repository, _strategy);
        _backup.AddRestorePoint(restorePoint);

        return restorePoint;
    }

    public ITrackableElement AddTrackableElement(ITrackableElement trackableElement)
    {
        ArgumentNullException.ThrowIfNull(trackableElement);

        _files.Add(trackableElement);

        return trackableElement;
    }

    public void RemoveTrackableElement(ITrackableElement trackableElement)
    {
        ArgumentNullException.ThrowIfNull(trackableElement);

        _files.Remove(trackableElement);
    }
}
