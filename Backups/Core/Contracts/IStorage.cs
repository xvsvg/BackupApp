using Backups.Contracts;

namespace Backups.Core.Contracts;

public interface IStorage
{
    IEnumerable<ITrackableElement> Files { get; }
}
