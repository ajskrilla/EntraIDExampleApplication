using EntraIDScanner.API.Models;
using Microsoft.Graph.Models;

namespace EntraIDScanner.API.RepositoryInterfaces.StandardInterfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<List<DirectoryObject>> GetByIdsAsync(IEnumerable<string> ids,
            IEnumerable<string> types);
        Task<User> GetUser(string userprincipal);
        public Task SaveUsersAsync(IEnumerable<StoredUser> users)
        {
            throw new NotImplementedException();
        }
    }
}
