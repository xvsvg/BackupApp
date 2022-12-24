using Backups.Contracts;
using Backups.Core.Contracts;
using Backups.Entities;
using Backups.Extra.Core.Algorithms.Contracts;
using Backups.Extra.Core.Logger.Contracts;
using Backups.Extra.Core.Strategies.Contracts;
using Zio;

namespace Backups.Extra.Core.Entities;

public class BackupTaskExtended
{
    private List<RestorePointExtended> _restorePoints;

    public BackupTaskExtended(
        BackupTask backupTask,
        AbstractRepository repository,
        IRestorePointFilter restrction,
        IBackupLogger logger,
        BackupSaver saver)
    {
        ArgumentNullException.ThrowIfNull(backupTask);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(restrction);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(saver);

        BackupTask = backupTask;
        Restrction = restrction;
        Logger = logger;
        Saver = saver;
        Repository = repository;

        _restorePoints = new List<RestorePointExtended>();
    }

    public BackupTask BackupTask { get; }
    public IRestorePointFilter Restrction { get; }
    public IBackupLogger Logger { get; }
    public BackupSaver Saver { get; }
    public AbstractRepository Repository { get; }
    public IEnumerable<RestorePointExtended> RestorePoints => _restorePoints;
    public IEnumerable<IStorage> Storages => BackupTask.Storages;
    public Backup Backup => BackupTask.Backup;
    public IEnumerable<ITrackableElement> TrackingFiles => BackupTask.TrackingFiles;

    public RestorePointExtended CreateRestorePoint(string storageName)
    {
        RestorePoint restorePoint = BackupTask.CreateRestorePointAndStorage(storageName);
        var result = new RestorePointExtended(restorePoint, DateTime.Now);

        _restorePoints.Add(result);

        Logger.Log($"Restore point made on {DateTime.Now}");
        Saver.SaveState();

        return result;
    }

    public ITrackableElement AddTrackableElement(ITrackableElement element)
    {
        BackupTask.AddTrackableElement(element);
        Logger.Log($"Added new trackable element: {element}");
        Saver.SaveState();

        return element;
    }

    public void RemoveTrackableElement(ITrackableElement element)
    {
        BackupTask.RemoveTrackableElement(element);
        Logger.Log($"Removed following trackable element: {element}");
        Saver.SaveState();
    }

    public RestorePointExtended AddRestorePoint(RestorePointExtended restorePoint)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);

        _restorePoints.Add(restorePoint);
        BackupTask.Backup.AddRestorePoint(restorePoint.RestorePoint);

        return restorePoint;
    }

    public void RemoveRestorePoint(RestorePointExtended restorePoint)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);

        _restorePoints.Remove(restorePoint);
    }

    public void CleanRestorePoints(IRestorePointFilter restriction, IRestorePointCleanStrategy cleanStrategy)
    {
        _restorePoints = cleanStrategy.Clean(restriction, RestorePoints).ToList();

        foreach (RestorePointExtended restorePointExtended in _restorePoints)
        {
            Backup.AddRestorePoint(restorePointExtended.RestorePoint);
        }

        Logger.Log("Cleaning finished sucessfully");
        Saver.SaveState();
    }

    public void RestoreState(RestorePointExtended restorePoint, string? restorePath = null)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);

        if (_restorePoints.Contains(restorePoint) is false)
            throw new Exception();

        Logger.Log($"Restoring from {restorePoint} ...");
        Saver.SaveState();

        if (restorePath is not null)
            RestoreToPath(restorePoint, restorePath);
        else RestoreToRepository(restorePoint);

        Logger.Log($"Sucessfully restored.");
    }

    private void RestoreToRepository(RestorePointExtended restorePoint)
    {
        foreach (ITrackableElement element in restorePoint.RestorePoint.Storage.SelectMany(storage => storage.Files))
        {
            if (element is BackupFile)
            {
                RestoreFile(element, element.ElementPath);
            }
            else if (element is BackupFolder)
            {
                RestoreFolder(element, element.ElementPath);
            }
        }
    }

    private void RestoreToPath(RestorePointExtended restorePoint, string path)
    {
        foreach (ITrackableElement element in restorePoint.RestorePoint.Storage.SelectMany(storage => storage.Files))
        {
            BackupTask.AddTrackableElement(element);

            if (element is BackupFile)
            {
                RestoreFile(element, path);
            }
            else if (element is BackupFolder)
            {
                RestoreFolder(element, path);
            }
        }
    }

    private void RestoreFile(ITrackableElement element, string restorePath)
    {
        Repository.FileSystem.CopyFile(
                            srcPath: Path.Combine(Repository.Path, element.Name),
                            destPath: Path.Combine(restorePath, element.Name),
                            overwrite: true);
    }

    private void RestoreFolder(ITrackableElement element, string restorePath)
    {
        Repository.FileSystem.CopyDirectory(
                            srcFolder: Path.Combine(Repository.Path, element.Name),
                            destFileSystem: Repository.FileSystem,
                            dstFolder: Path.Combine(restorePath, element.Name),
                            overwrite: true);
    }
}