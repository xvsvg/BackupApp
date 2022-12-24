using Backups.Contracts;

namespace Backups.Entities;

public class BackupFile : ITrackableElement
{
    private readonly Stream _fileStream;

    public BackupFile(string path, string name, AbstractRepository repository)
    {
        ElementPath = path;
        Name = name;
        _fileStream = repository.Open(Path.Combine(ElementPath, Name));
        _fileStream.Dispose();
    }

    public string ElementPath { get; private set; }
    public string Name { get; private set; }
    public Stream FileStream => _fileStream;

    public override string ToString()
        => $"..\\{Name}";

    public void Accept(ITrackableElementVisitor visitor)
        => visitor.VisitFile(this);
}
