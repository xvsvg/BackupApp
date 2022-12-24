using Backups.Core.Contracts;
using Backups.Entities;
using Backups.Infrastructure.Implementaions.Algorithms;
using Backups.Infrastructure.Implementaions.Repositories;
using Backups.UI;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Backups.Test;

public class BackupProviderTest
{
    private static string workingDirectory = @"D:\git repositories\xvsvg\Lab3\Backups.Test";
    private static IArchiver archiver = new Archiver();
    private readonly BackupProvider _backupProvider = new BackupProvider();
    private readonly SingleStorageAlgorithm singleStorage = new SingleStorageAlgorithm(archiver);
    private readonly SplitStorageAlgorithm splitStorage = new SplitStorageAlgorithm(archiver);

    [Fact]
    public void MemoryFileSystemRepositoryTest()
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

        task.AddTrackableElement(folder1);
        task.AddTrackableElement(file1);
        task.AddTrackableElement(file2);
        task.CreateRestorePointAndStorage("archived");

        Assert.Single(task.Backup.RestorePoints);
    }

    internal void PhysicalFileSystemRepositoryTest_SingleStorage()
    {
        var fs = new PhysicalFileSystem();
        var repository = new PhysicalFileSystemRepository(workingDirectory, fs);
        var task = new BackupTask("Task1", repository, singleStorage);

        FileStream stream1 = File.Create(Path.Combine(workingDirectory, "TextFile1.txt"));
        stream1.Dispose();
        File.WriteAllText(Path.Combine(workingDirectory, "TextFile1.txt"), "TextFile1");

        FileStream stream2 = File.Create(Path.Combine(workingDirectory, "TextFile2.txt"));
        stream2.Dispose();
        File.WriteAllText(Path.Combine(workingDirectory, "TextFile2.txt"), "TextFile2");

        DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(workingDirectory, "Folder"));

        FileStream stream3 = File.Create(Path.Combine(dir.FullName, "FolderFile.txt"));
        stream3.Dispose();
        File.WriteAllText(Path.Combine(dir.FullName, "FolderFile.txt"), "FolderFile");

        var file1 = new BackupFile(workingDirectory, "TextFile1.txt", repository);
        var file2 = new BackupFile(workingDirectory, "TextFile2.txt", repository);
        var folder1 = new BackupFolder(workingDirectory, "Folder", repository);

        task.AddTrackableElement(folder1);
        task.AddTrackableElement(file1);
        task.AddTrackableElement(file2);
        task.CreateRestorePointAndStorage("archived1");

        task.CreateRestorePointAndStorage("archived2");

        task.RemoveTrackableElement(file2);

        task.CreateRestorePointAndStorage("archived3");

        Assert.Equal(2, task.TrackingFiles.Count());
        Assert.Equal(3, task.Storages.Count());
    }

    internal void PhysicalFileSystemRepositoryTest_SplitStorage()
    {
        var fs = new PhysicalFileSystem();
        var repository = new PhysicalFileSystemRepository(workingDirectory, fs);
        var task = new BackupTask("Task1", repository, splitStorage);

        var file1 = new BackupFile(workingDirectory, "TextFile1.txt", repository);
        var file2 = new BackupFile(workingDirectory, "TextFile2.txt", repository);
        var folder1 = new BackupFolder(workingDirectory, "Folder", repository);

        task.AddTrackableElement(folder1);
        task.AddTrackableElement(file1);
        task.AddTrackableElement(file2);
        task.CreateRestorePointAndStorage("archived");

        Assert.Equal(3, task.TrackingFiles.Count());
        Assert.Equal(3, task.Storages.Count());
    }
}
