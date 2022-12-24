using System.IO.Compression;
using Backups.Contracts;
using Backups.Core.Contracts;
using Backups.Core.Entities;
using Backups.Infrastructure.Implementaions.Visitors;

namespace Backups.Entities;

public class Archiver : IArchiver
{
    public IStorage Archive(IEnumerable<ITrackableElement> files, AbstractRepository repository, string archiveName)
    {
        ArgumentNullException.ThrowIfNull(files);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(archiveName);

        using Stream zipStream = repository.Open(Path.Combine(repository.Path, archiveName + ".zip"));

        var visitor = new ZipArchiveVisitor(repository);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create);
        visitor.AddArchive(archive);

        var storage = new Storage();
        foreach (ITrackableElement element in files)
        {
            element.Accept(visitor);
            storage.AddElement(element);
        }

        return storage;
    }
}