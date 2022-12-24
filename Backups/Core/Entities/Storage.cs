using Backups.Contracts;
using Backups.Core.Contracts;

namespace Backups.Core.Entities;

public class Storage : IStorage
{
    private readonly List<ITrackableElement> _files;

    public Storage()
    {
        _files = new List<ITrackableElement>();
    }

    public IEnumerable<ITrackableElement> Files => _files;

    public void AddElement(ITrackableElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        _files.Add(element);
    }
}
