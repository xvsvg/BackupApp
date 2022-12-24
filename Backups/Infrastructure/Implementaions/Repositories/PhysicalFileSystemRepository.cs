using Backups.Contracts;
using Backups.Core.Exceptions;
using Backups.Entities;
using Zio.FileSystems;

namespace Backups.Infrastructure.Implementaions.Repositories;

public class PhysicalFileSystemRepository : AbstractRepository
{
    public PhysicalFileSystemRepository(string repositoryPath, FileSystem fileSystem)
        : base(repositoryPath, fileSystem) { }

    public override ITrackableElement GetElement(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (FileSystem.FileExists(path))
            return new BackupFile(new FileInfo(path).FullName, new FileInfo(path).Name, this);
        if (FileSystem.DirectoryExists(path))
            return new BackupFolder(new DirectoryInfo(path).FullName, new DirectoryInfo(path).Name, this);

        throw RepositoryExceptions.NotSupportedPathException("couldn't work out path");
    }

    public override Stream Open(string path)
    {
        if (FileSystem.FileExists(path))
            return FileSystem.OpenFile(path, FileMode.Open, FileAccess.ReadWrite);

        throw new Exception();
    }
}
