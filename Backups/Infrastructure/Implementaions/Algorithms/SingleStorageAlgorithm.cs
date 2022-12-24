using Backups.Contracts;
using Backups.Core.Contracts;

namespace Backups.Infrastructure.Implementaions.Algorithms;

public class SingleStorageAlgorithm : IStorageAlgorithm
{
    private readonly IArchiver _archiver;

    public SingleStorageAlgorithm(IArchiver archiver)
    {
        ArgumentNullException.ThrowIfNull(archiver);
        _archiver = archiver;
    }

    public IEnumerable<IStorage> CreateStorage(IEnumerable<ITrackableElement> files, AbstractRepository repository, string storageName)
    {
        ArgumentNullException.ThrowIfNull(files);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(storageName);

        yield return _archiver.Archive(files, repository, storageName);
    }
}
