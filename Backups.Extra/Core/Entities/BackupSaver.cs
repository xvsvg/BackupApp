using Backups.Contracts;
using Newtonsoft.Json;
using Zio;

namespace Backups.Extra.Core.Entities;

public class BackupSaver
{
    private List<BackupTaskExtended> _backupTasks;

    public BackupSaver(AbstractRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _backupTasks = new List<BackupTaskExtended>();
        Repository = repository;
    }

    public AbstractRepository Repository { get; }

    public void SaveState()
    {
        Stream stream = Repository.FileSystem.CreateFile(Path.Combine(Repository.Path, ".config"));
        stream.Dispose();

        var serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        };

        Repository.FileSystem.WriteAllText(
            Path.Combine(Repository.Path, ".config"),
            JsonConvert.SerializeObject(_backupTasks, Formatting.Indented, serializerSettings));
    }

    public IEnumerable<BackupTaskExtended> UpdateState()
    {
        var serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        };

        string text = Repository.FileSystem.ReadAllText(Path.Combine(Repository.Path, ".config"));
        List<BackupTaskExtended>? output = JsonConvert.DeserializeObject<List<BackupTaskExtended>>(text, serializerSettings);

        if (output is null)
            throw new Exception();

        _backupTasks = output;

        return _backupTasks;
    }

    public BackupTaskExtended AddBackupTask(BackupTaskExtended backupTask)
    {
        ArgumentNullException.ThrowIfNull(backupTask);

        _backupTasks.Add(backupTask);

        return backupTask;
    }
}