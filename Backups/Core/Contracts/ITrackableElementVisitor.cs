using Backups.Entities;

namespace Backups.Contracts;

public interface ITrackableElementVisitor
{
    void VisitFolder(BackupFolder folder);
    void VisitFile(BackupFile file);
}
