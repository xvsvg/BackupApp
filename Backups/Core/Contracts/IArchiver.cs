using Backups.Contracts;

namespace Backups.Core.Contracts;

public interface IArchiver
{
    IStorage Archive(IEnumerable<ITrackableElement> files, AbstractRepository repository, string archiveName);
}
