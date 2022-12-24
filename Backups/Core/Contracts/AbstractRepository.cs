using Zio.FileSystems;

namespace Backups.Contracts;

public abstract class AbstractRepository : IDisposable
{
    private readonly FileSystem _fileSystem;

    public AbstractRepository(string path, FileSystem fileSystem)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(fileSystem);

        _fileSystem = fileSystem;
        Path = path;
    }

    public string Path { get; private set; }
    public FileSystem FileSystem => _fileSystem;
    public abstract Stream Open(string path);
    public abstract ITrackableElement GetElement(string path);

    public void Dispose()
    {
        _fileSystem.Dispose();
    }
}
