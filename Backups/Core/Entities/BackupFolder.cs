using Backups.Contracts;
using Backups.Core.Exceptions;
using Zio;

namespace Backups.Entities;

public class BackupFolder : ITrackableElement
{
    private readonly List<ITrackableElement> _files;
    private readonly List<Stream> _folderContentStreams;

    public BackupFolder(string path, string name, AbstractRepository repository)
    {
        ElementPath = path;
        Name = name;
        _files = new List<ITrackableElement>();
        _folderContentStreams = new List<Stream>();

        InitializeFolderFiles(path, name, repository);

        foreach (ITrackableElement file in _files)
        {
            Stream fileStream = repository.Open(Path.Combine(file.ElementPath, file.Name));
            _folderContentStreams.Add(fileStream);
            fileStream.Dispose();
        }
    }

    public string ElementPath { get; private set; }
    public string Name { get; private set; }
    public IEnumerable<ITrackableElement> Files => _files;
    public IEnumerable<Stream> FolderContentStream => _folderContentStreams;

    public override string ToString()
        => $"..\\{Name}";

    public void Accept(ITrackableElementVisitor visitor)
        => visitor.VisitFolder(this);

    private void InitializeFolderFiles(string path, string name, AbstractRepository repository)
    {
        string elementName, elementPath;
        foreach (string element in repository.FileSystem.EnumeratePaths(Path.Combine(path, name)))
        {
            (elementPath, elementName) = ParseElementPath(element);

            if (repository.FileSystem.FileExists(element))
                _files.Add(new BackupFile(elementPath, elementName, repository));
            if (repository.FileSystem.DirectoryExists(element))
                _files.Add(new BackupFolder(elementPath, elementName, repository));
        }
    }

    private (string elementPath, string elementName) ParseElementPath(string? elementPath)
    {
        ArgumentNullException.ThrowIfNull(elementPath);

        for (int i = elementPath.Length - 1; i >= 0; --i)
        {
            if (char.IsLetter(elementPath[i]) is false && elementPath[i] != '.')
                return (elementPath[0..i], elementPath[++i..]);
        }

        throw BackupFolderExceptions.NotSupportPathException("couldn't work out path");
    }
}