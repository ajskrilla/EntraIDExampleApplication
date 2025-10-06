namespace EntraIDScanner.API.RepositoryInterfaces.StoredDatabaseRepository
{
    using EntraIDScanner.API.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IStoredDatabaseDeviceRepository
    {
        Task<List<StoredDevice>> GetAllAsync();
        Task<StoredDevice> GetByIdAsync(string azureId);
        Task SaveManyAsync(IEnumerable<StoredDevice> devices);
        Task DeleteAllAsync();
    }
}
