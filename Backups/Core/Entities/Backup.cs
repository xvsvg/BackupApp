using Backups.Contracts;

namespace Backups.Entities;

public class Backup : IBackup
{
    private readonly List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
        Id = Guid.NewGuid();
    }

    public Guid Id { get; private set; }
    public IEnumerable<RestorePoint> RestorePoints => _restorePoints;

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);

        _restorePoints.Add(restorePoint);
    }
}
