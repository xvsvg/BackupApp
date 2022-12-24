using Backups.Contracts;
using Backups.Core.Contracts;
using Backups.Tools;

namespace Backups.Infrastructure.Implementaions.Algorithms;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    private readonly IArchiver _archiver;

    public SplitStorageAlgorithm(IArchiver archiver)
    {
        ArgumentNullException.ThrowIfNull(archiver);
        _archiver = archiver;
    }

    public IEnumerable<IStorage> CreateStorage(IEnumerable<ITrackableElement> files, AbstractRepository repository, string storageName)
    {
        ArgumentNullException.ThrowIfNull(files);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(storageName);

        var versionGenerator = new VersionGenerator();
        foreach (ITrackableElement file in files)
        {
            yield return _archiver.Archive(new List<ITrackableElement> { file }, repository, storageName + versionGenerator.Generate());
        }
    }
}
