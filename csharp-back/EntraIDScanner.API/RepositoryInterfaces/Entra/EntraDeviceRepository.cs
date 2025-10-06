using EntraIDScanner.API.RepositoryInterfaces.StandardInterfaces;
using EntraIDScanner.API.Services.EntraId;
using Microsoft.Graph.Devices;
//using Microsoft.Graph.Devices.GetByIds;
using Microsoft.Graph.DirectoryObjects.GetByIds;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;

namespace EntraIDScanner.API.RepositoryInterfaces.Entra
{
    public class EntraDeviceRepository : IDeviceRepository
    {
        private readonly ILogger<EntraDeviceRepository> _logger;
        private readonly GraphService _graphService;
        private readonly CredentialService _credentialService;

        public EntraDeviceRepository(
        ILogger<EntraDeviceRepository> logger,
        GraphService graphService,
        CredentialService credentialService)
        {
            _logger = logger;
            _graphService = graphService;
            _credentialService = credentialService;

        }
        public async Task<List<DirectoryObject>> GetByIdsAsync(
            IEnumerable<string> ids,
            IEnumerable<string> types)
        {
            _logger.LogInformation("Need to get ID of device.");
            var cred = _credentialService.GetLatestCredential();
            var client = _graphService.GetGraphClient(cred);
            var body = new GetByIdsPostRequestBody
            {
                Ids = ids.ToList(),
                Types = types.ToList()
            };
           
            var result = await client
                                 .DirectoryObjects
                                 .GetByIds.PostAsGetByIdsPostResponseAsync(body);
            return result.Value.ToList();
        }

        public async Task<Device> GetDevice(string azureId)
        {
            //get cred from DB
            var cred = _credentialService.GetLatestCredential();
            if (cred == null)
            {
                _logger.LogError("No credentials available when trying to Get All Users.");
                return null;
            }
            //init client
            var client = _graphService.GetGraphClient(cred);
            _logger.LogInformation($"Fetching a single device from Microsoft Graph...");
            try
            {
                _logger.LogInformation($"Fetching by azId = '{azureId}'");
                return await client.Devices[azureId].GetAsync();
            }
            catch (ApiException ex) when (ex.ResponseStatusCode == 404)
            {
                _logger.LogInformation("Key lookup 404 for '{Key}', falling back to filter", azureId);
                _logger.LogInformation($"Direct lookup failed for '{azureId}', trying filter fallback. This will look up via Displayname");

                //This is the query to look up device via dispalyname in Entra
                var filter = $"id eq '{azureId}' or startswith(displayName,'{azureId}')";
                var page = await client.Devices.GetAsync(cfg => {
                    cfg.QueryParameters.Filter = filter;
                    cfg.QueryParameters.Select = new[] { "id", "displayName", "deviceId", "operatingSystem" };
                });
                //need to test query to look for device
                var found = page.Value.FirstOrDefault();
                if (found != null)
                {
                    _logger.LogInformation("Filter lookup succeeded for machine: {Id}", found.Id);
                    return found;
                }

                _logger.LogWarning("No machine matched by filter for '{Key}'.", azureId);
                return null;
            }
            //copy the above exception method when looking for the user with another query
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching a user leveraging userprincipal name in Microsoft Graph.");
                return null;
            }
        }

        public async Task<List<Device>> GetAllDevicesAsync()
        {
            var allDevices = new List<Device>();
            var cred = _credentialService.GetLatestCredential();
            if (cred == null)
            {
                _logger.LogError("No credentials available when trying to Get All Devices.");
                return null;
            }
            //init client
            var client = _graphService.GetGraphClient(cred);
            try
            {
                _logger?.LogInformation("Fetching all Devices (handling pagination)...");

                var page = await client.Devices.GetAsync();
                int pageNumber = 1;

                while (page != null)
                {
                    _logger?.LogInformation($"Fetched page {pageNumber} with {page.Value.Count} devices.");
                    allDevices.AddRange(page.Value);

                    if (!string.IsNullOrEmpty(page.OdataNextLink))
                    {
                        page = await new DevicesRequestBuilder(page.OdataNextLink, client.RequestAdapter)
                                        .GetAsync();
                        pageNumber++;
                    }
                    else
                    {
                        break;
                    }
                }
                _logger?.LogInformation($"Total users retrieved: {allDevices.Count}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while paginating users.");
                throw;
            }
            return allDevices;
        }
    }
}
