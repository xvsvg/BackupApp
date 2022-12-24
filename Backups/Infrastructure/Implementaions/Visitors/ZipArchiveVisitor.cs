using System.IO.Compression;
using Backups.Contracts;
using Backups.Entities;

namespace Backups.Infrastructure.Implementaions.Visitors;

public class ZipArchiveVisitor : ITrackableElementVisitor
{
    private readonly Stack<ZipArchive> _zipArchives;
    private readonly AbstractRepository _repository;

    public ZipArchiveVisitor(AbstractRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
        _zipArchives = new Stack<ZipArchive>();
    }

    public ZipArchive Peek => _zipArchives.Peek();

    public void VisitFile(BackupFile file)
    {
        ArgumentNullException.ThrowIfNull(file);

        ZipArchive archive = Peek;
        using Stream fileStream = _repository.Open(Path.Combine(file.ElementPath, file.Name));

        using Stream entryStream = archive.CreateEntry(file.Name).Open();
        fileStream.CopyTo(entryStream);
    }

    public void VisitFolder(BackupFolder folder)
    {
        ArgumentNullException.ThrowIfNull(folder);

        ZipArchive archive = Peek;
        using Stream entryStream = archive.CreateEntry(folder.Name + ".zip").Open();

        using var folderArchive = new ZipArchive(entryStream, ZipArchiveMode.Create);
        _zipArchives.Push(folderArchive);

        foreach (ITrackableElement file in folder.Files)
        {
            file.Accept(this);
        }

        _zipArchives.Pop();
    }

    public void AddArchive(ZipArchive archive)
    {
        ArgumentNullException.ThrowIfNull(archive);

        _zipArchives.Push(archive);
    }
}
