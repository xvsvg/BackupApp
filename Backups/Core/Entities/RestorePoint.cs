using Backups.Contracts;
using Backups.Core.Contracts;

namespace Backups.Entities;

public class RestorePoint
{
    private readonly IEnumerable<IStorage> _storages;
    private readonly AbstractRepository _repository;

    public RestorePoint(IEnumerable<IStorage> storages, AbstractRepository repository, IStorageAlgorithm saveStrategy)
    {
        ArgumentNullException.ThrowIfNull(storages);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(saveStrategy);

        _storages = storages;
        _repository = repository;
        TimeStamp = DateTime.Now;
    }

    public DateTime TimeStamp { get; private set;  }
    public IEnumerable<IStorage> Storage => _storages;
}
