namespace EntraIDScanner.API.RepositoryInterfaces.StoredDatabaseRepository
{
    using EntraIDScanner.API.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStoredDatabaseUserRepository
    {
        Task<List<StoredUser>> GetAllAsync();
        Task<StoredUser> GetByIdAsync(string azureId);
        Task SaveManyAsync(IEnumerable<StoredUser> users);
        Task DeleteAllAsync();
    }
}

