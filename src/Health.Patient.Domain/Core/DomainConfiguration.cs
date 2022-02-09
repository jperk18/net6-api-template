using Health.Patient.Storage.Sql.Core;

namespace Health.Patient.Domain.Core;

public interface IDomainConfiguration
{
    IStorageConfiguration StorageConfiguration { get; set; }
}

public class DomainConfiguration : IDomainConfiguration
{
    public DomainConfiguration(IStorageConfiguration storageConfiguration)
    {
        StorageConfiguration = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
    }
    public IStorageConfiguration StorageConfiguration { get; set; }
}