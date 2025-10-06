using System;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Users;
using Microsoft.Graph.Devices;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;

namespace EntraIDScanner.API.Services.EntraId
{
    public class GraphService
    {
        private readonly ILogger<GraphService> _logger;
        private GraphServiceClient _graphClient;

        public GraphService(ILogger<GraphService> logger)
        {
            _logger = logger;
        }
        public GraphServiceClient GetGraphClient(Credential credential)
        {
            _logger.LogInformation(" Building GraphServiceClient from provided credentials...");

            if (_graphClient == null)
            {
                try
                {
                    var azureCredential = new ClientSecretCredential(
                        credential.TenantId,
                        credential.ClientId,
                        credential.ClientSecret,
                        new TokenCredentialOptions
                        {
                            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                        });

                    _graphClient = new GraphServiceClient(azureCredential);

                    _logger.LogInformation(" Graph client successfully initialized.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, " Error initializing Microsoft Graph client.");
                    throw;
                }
            }

            return _graphClient;
        }
    }
}


