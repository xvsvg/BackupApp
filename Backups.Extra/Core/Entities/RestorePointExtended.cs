using Backups.Entities;

namespace Backups.Extra.Core.Entities;

public class RestorePointExtended
{
    public RestorePointExtended(RestorePoint restorePoint, DateTime date)
    {
        RestorePoint = restorePoint;
        Date = date;
    }

    public RestorePoint RestorePoint { get; }
    public DateTime Date { get; }
}