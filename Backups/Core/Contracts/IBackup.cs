using Backups.Entities;

namespace Backups.Contracts;

public interface IBackup
{
    IEnumerable<RestorePoint> RestorePoints { get; }
    void AddRestorePoint(RestorePoint restorePoint);
}
