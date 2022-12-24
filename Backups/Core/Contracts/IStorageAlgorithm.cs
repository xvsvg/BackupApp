using Backups.Core.Contracts;

namespace Backups.Contracts;

public interface IStorageAlgorithm
{
    IEnumerable<IStorage> CreateStorage(IEnumerable<ITrackableElement> files, AbstractRepository repository, string storageName);
}
