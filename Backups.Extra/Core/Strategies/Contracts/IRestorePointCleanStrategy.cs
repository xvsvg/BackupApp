using Backups.Extra.Core.Algorithms.Contracts;
using Backups.Extra.Core.Entities;

namespace Backups.Extra.Core.Strategies.Contracts;

public interface IRestorePointCleanStrategy
{
    IEnumerable<RestorePointExtended> Clean(
        IRestorePointFilter restriction,
        IEnumerable<RestorePointExtended> restorePoints);
}