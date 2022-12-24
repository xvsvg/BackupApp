using Backups.Contracts;
using Backups.Extra.Core.Logger.Contracts;
using Zio;

namespace Backups.Extra.Core.Logger.Implementations;

public class FileLogger : IBackupLogger
{
    private readonly AbstractRepository _repository;

    public FileLogger(AbstractRepository repository, string fileName, bool enableDatePrefix = false)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
        FileName = fileName;
        EnableDatePrefix = enableDatePrefix;
    }

    public string FileName { get; }
    public bool EnableDatePrefix { get; }

    public void Log(string v)
    {
        if (EnableDatePrefix)
            WriteDatePrefix();

        _repository.FileSystem.WriteAllText(Path.Combine(_repository.Path, FileName), v);
    }

    private void WriteDatePrefix()
    {
        _repository.FileSystem
            .WriteAllText(
            Path.Combine(
            _repository.Path, FileName),
            $"[{DateTime.Now}]");
    }
}