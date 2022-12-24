using Backups.Extra.Core.Algorithms.Contracts;
using Backups.Extra.Core.Entities;
using Backups.Extra.Core.Strategies.Contracts;

namespace Backups.Extra.Core.Strategies.Implementations;

public class StrategyAll : IRestorePointCleanStrategy
{
    public IEnumerable<RestorePointExtended> Clean(IRestorePointFilter restriction, IEnumerable<RestorePointExtended> restorePoints)
    {
        ArgumentNullException.ThrowIfNull(restorePoints);
        ArgumentNullException.ThrowIfNull(restriction);

        return restriction.Filter(restorePoints);
    }
}