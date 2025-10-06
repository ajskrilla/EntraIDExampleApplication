using EntraIDScanner.API.Models;
using Microsoft.Graph.Models;

namespace EntraIDScanner.API.RepositoryInterfaces.StandardInterfaces
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetAllDevicesAsync();
        Task<Device> GetDevice(string deviceName);
        Task<List<DirectoryObject>> GetByIdsAsync(IEnumerable<string> ids,
            IEnumerable<string> types);
        public Task SaveDevicesAsync(IEnumerable<Device> devices)
        {
            throw new NotImplementedException();
        }
    }
}
