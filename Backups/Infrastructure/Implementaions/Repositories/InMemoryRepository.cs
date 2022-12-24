using Backups.Contracts;
using Backups.Core.Exceptions;
using Backups.Entities;
using Zio.FileSystems;

namespace Backups.Infrastructure.Implementaions.Repositories;

public class InMemoryRepository : AbstractRepository
{
    public InMemoryRepository(string path, MemoryFileSystem fileSystem)
        : base(path, fileSystem) { }

    public override ITrackableElement GetElement(string path)
    {
        string elementPath, elementName;

        (elementPath, elementName) = ParsePath(path);

        if (FileSystem.FileExists(path))
            return new BackupFile(elementPath, elementName, this);
        if (FileSystem.DirectoryExists(path))
            return new BackupFolder(elementPath, elementName, this);

        throw RepositoryExceptions.NotSupportedPathException("couldn't work out path");
    }

    public override Stream Open(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        return FileSystem.OpenFile(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    private (string elementPath, string elementName) ParsePath(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        for (int i = path.Length - 1; i >= 0; --i)
        {
            if (char.IsLetter(path[i]) is false && path[i] != '.')
                return (path[0..i], path[++i..]);
        }

        throw RepositoryExceptions.NotSupportedPathException("couldn't work out path");
    }
}
