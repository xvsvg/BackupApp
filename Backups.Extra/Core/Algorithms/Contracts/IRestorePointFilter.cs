using Backups.Extra.Core.Entities;

namespace Backups.Extra.Core.Algorithms.Contracts;

public interface IRestorePointFilter
{
    IEnumerable<RestorePointExtended> Filter(IEnumerable<RestorePointExtended> restorePoints);
}