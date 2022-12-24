using Backups.Extra.Core.Algorithms.Contracts;
using Backups.Extra.Core.Entities;

namespace Backups.Extra.Core.Algorithms.Implementations;

public class RestorePointQuantityFilter : IRestorePointFilter
{
    public RestorePointQuantityFilter(int quantity)
    {
        Quantity = quantity;
    }

    public int Quantity { get; }

    public IEnumerable<RestorePointExtended> Filter(IEnumerable<RestorePointExtended> restorePoints)
        => restorePoints.TakeLast(Quantity).ToList();
}