using Backups.Extra.Core.Algorithms.Contracts;
using Backups.Extra.Core.Entities;

namespace Backups.Extra.Core.Algorithms.Implementations;

public class RestorePointDateFilter : IRestorePointFilter
{
    public RestorePointDateFilter(DateTime dateTime)
    {
        Date = dateTime;
    }

    public DateTime Date { get; }

    public IEnumerable<RestorePointExtended> Filter(IEnumerable<RestorePointExtended> restorePoints)
        => restorePoints.Where(x => x.Date > Date).ToList();
}