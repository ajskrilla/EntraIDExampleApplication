using EntraIDScanner.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.DirectoryObjects.GetByIds;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Kiota.Abstractions;
using Microsoft.Graph.Users;
using EntraIDScanner.API.RepositoryInterfaces.StandardInterfaces;
using EntraIDScanner.API.Services.EntraId;

namespace EntraIDScanner.API.RepositoryInterfaces.Entra
{
    public class EntraUserRepository : IUserRepository
    {
        // These dependencies should be injected via DI:
        private readonly ILogger<EntraUserRepository> _logger;
        private readonly GraphService _graphService;
        private readonly CredentialService _credentialService;

        // The constructor now takes the needed dependencies.

        //Create a way to instantiate credentials so that its referenced in one place and can be accessed through the main instantiation
        public EntraUserRepository(
            ILogger<EntraUserRepository> logger,
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
            _logger.LogInformation("Need to get IDs.");
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
        public async Task<List<User>> GetAllUsersAsync()
        {
            var allUsers = new List<User>();
            //get cred from DB
            var cred = _credentialService.GetLatestCredential();
            if (cred == null)
            {
                _logger.LogError("No credentials available when trying to Get All Users.");
                return null;
            }
            //init client
            var client = _graphService.GetGraphClient(cred);
            try
            {
                _logger?.LogInformation("Fetching all users (handling pagination)...");

                var page = await client.Users.GetAsync();
                int pageNumber = 1;

                while (page != null)
                {
                    _logger?.LogInformation($"Fetched page {pageNumber} with {page.Value.Count} users.");
                    allUsers.AddRange(page.Value);

                    if (!string.IsNullOrEmpty(page.OdataNextLink))
                    {
                        page = await new UsersRequestBuilder(page.OdataNextLink, client.RequestAdapter)
                                        .GetAsync();
                        pageNumber++;
                    }
                    else
                    {
                        break;
                    }
                }
                _logger?.LogInformation($"Total users retrieved: {allUsers.Count}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while paginating users.");
                throw;
            }
            return allUsers;
        }
        public async Task<User> GetUser(string userprincipal)
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
            _logger.LogInformation($"Fetching a single user from Microsoft Graph...");
            try
            {
                _logger.LogInformation($"Fetching by userPrincipalName = '{userprincipal}'");
                return await client.Users[userprincipal].GetAsync();
            }
            catch (ApiException ex) when (ex.ResponseStatusCode == 404)
            {
                _logger.LogInformation("Key lookup 404 for '{Key}', falling back to filter", userprincipal);
                _logger.LogInformation($"Direct lookup failed for '{userprincipal}', trying filter fallback. This will look up user via Email OR mail address");

                //This is the query to look up user principal or email address in Entra
                var filter = $"userPrincipalName eq '{userprincipal}' or mail eq '{userprincipal}'";
                var page = await client.Users.GetAsync(cfg =>
                {
                    cfg.QueryParameters.Filter = filter;
                    // optionally select only the fields you need:
                    cfg.QueryParameters.Select = new[] { "id", "displayName", "userPrincipalName", "mail" };
                });

                var found = page.Value.FirstOrDefault();
                if (found != null)
                {
                    _logger.LogInformation("Filter lookup succeeded: {UPN} / {Id}", found.UserPrincipalName, found.Id);
                    return found;
                }

                _logger.LogWarning("No user matched by filter for '{Key}'.", userprincipal);
                return null;
            }
            //copy the above exception method when looking for the user with another query
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching a user leveraging userprincipal name in Microsoft Graph.");
                return null;
            }
        }
    }
}
