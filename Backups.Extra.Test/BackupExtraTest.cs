using Backups.Core.Contracts;
using Backups.Entities;
using Backups.Extra.Core.Algorithms.Implementations;
using Backups.Extra.Core.Entities;
using Backups.Extra.Core.Logger.Implementations;
using Backups.Extra.Core.Strategies.Implementations;
using Backups.Infrastructure.Implementaions.Algorithms;
using Backups.Infrastructure.Implementaions.Repositories;
using Backups.UI;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Backups.Extra.Test;

public class BackupExtraTest
{
    private static IArchiver archiver = new Archiver();
    private readonly BackupProvider _backupProvider = new BackupProvider();
    private readonly SingleStorageAlgorithm singleStorage = new SingleStorageAlgorithm(archiver);
    private readonly SplitStorageAlgorithm splitStorage = new SplitStorageAlgorithm(archiver);

    [Fact]
    public void MemoryFileSystemRepositoryTest_RestorePointFilterWorks()
    {
        var fs = new MemoryFileSystem();
        fs.CreateDirectory(@"/home/runner/work/xvsvg/test");

        var repository = new InMemoryRepository(@"/home/runner/work/xvsvg/test", fs);
        var task = new BackupTask("Task1", repository, singleStorage);

        Stream stream1 = fs.CreateFile(@"/home/runner/work/xvsvg/test/TextFile1.txt");
        Stream stream2 = fs.CreateFile(@"/home/runner/work/xvsvg/test/TextFile2.txt");
        fs.CreateDirectory(@"/home/runner/work/xvsvg/test/Folder/");
        Stream stream3 = fs.CreateFile(@"/home/runner/work/xvsvg/test/Folder/FolderFile.txt");

        stream1.Dispose();
        stream2.Dispose();
        stream3.Dispose();

        var file1 = new BackupFile(@"/home/runner/work/xvsvg/test", "TextFile1.txt", repository);
        var file2 = new BackupFile(@"/home/runner/work/xvsvg/test", "TextFile2.txt", repository);
        var folder1 = new BackupFolder(@"/home/runner/work/xvsvg/test", "Folder", repository);

        var extTask = new BackupTaskExtended(task, repository, new RestorePointQuantityFilter(1), new ConsoleLogger(true), new BackupSaver(repository));

        extTask.AddTrackableElement(folder1);
        extTask.AddTrackableElement(file1);
        extTask.AddTrackableElement(file2);
        extTask.CreateRestorePoint("archived");

        extTask.CreateRestorePoint("archived1");
        extTask.CleanRestorePoints(extTask.Restrction, new StrategyAll());

        string str = repository.FileSystem.ReadAllText(Path.Combine(repository.Path, ".config"));

        Assert.Single(extTask.RestorePoints);
        Assert.NotEmpty(str);
    }

    internal void MemoryFileSystemRepositoryTest_RestorePointCanBeRestored()
    {
        var fs = new MemoryFileSystem();
        fs.CreateDirectory(@"/home/runner/work/xvsvg/test");

        var repository = new InMemoryRepository(@"/home/runner/work/xvsvg/test", fs);
        var task = new BackupTask("Task1", repository, singleStorage);

        Stream stream1 = fs.CreateFile(@"/home/runner/work/xvsvg/test/TextFile1.txt");
        Stream stream2 = fs.CreateFile(@"/home/runner/work/xvsvg/test/TextFile2.txt");
        fs.CreateDirectory(@"/home/runner/work/xvsvg/test/Folder/");
        Stream stream3 = fs.CreateFile(@"/home/runner/work/xvsvg/test/Folder/FolderFile.txt");

        stream1.Dispose();
        stream2.Dispose();
        stream3.Dispose();

        var file1 = new BackupFile(@"/home/runner/work/xvsvg/test", "TextFile1.txt", repository);
        var file2 = new BackupFile(@"/home/runner/work/xvsvg/test", "TextFile2.txt", repository);
        var folder1 = new BackupFolder(@"/home/runner/work/xvsvg/test", "Folder", repository);

        var extTask = new BackupTaskExtended(task, repository, new RestorePointQuantityFilter(2), new ConsoleLogger(true), new BackupSaver(repository));

        extTask.AddTrackableElement(folder1);
        extTask.AddTrackableElement(file1);
        extTask.AddTrackableElement(file2);
        extTask.CreateRestorePoint("archived");

        fs.WriteAllText(@"/home/runner/work/xvsvg/test/TextFile2.txt", "this is the content");
        RestorePointExtended rp = extTask.CreateRestorePoint("archived1");

        fs.DeleteFile(@"/home/runner/work/xvsvg/test/TextFile2.txt");
        Stream stream = fs.CreateFile(@"/home/runner/work/xvsvg/test/TextFile2.txt");
        stream.Dispose();
        extTask.CreateRestorePoint("archived2");

        extTask.CleanRestorePoints(extTask.Restrction, new StrategyAll());

        extTask.RestoreState(rp);
        string fileString = repository.FileSystem.ReadAllText(@"/home/runner/work/xvsvg/test/TextFile2.txt");

        Assert.Equal("this is the content", fileString);
    }
}