using Backups.Extra.Core.Algorithms.Contracts;
using Backups.Extra.Core.Entities;

namespace Backups.Extra.Core.Algorithms.Implementations;

public class RestorePointHybridFilter : IRestorePointFilter
{
    public RestorePointHybridFilter(Func<RestorePointExtended, bool> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        Action = action;
    }

    public Func<RestorePointExtended, bool> Action { get; }

    public IEnumerable<RestorePointExtended> Filter(IEnumerable<RestorePointExtended> restorePoints)
        => restorePoints.Where(x => Action(x)).ToList();
}