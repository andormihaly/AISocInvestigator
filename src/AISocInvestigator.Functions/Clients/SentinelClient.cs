using AISocInvestigator.Domain.Incidents;
using AISocInvestigator.Functions.Authentication;
using AISocInvestigator.Functions.Configuration;
using Microsoft.Extensions.Options;

namespace AISocInvestigator.Functions.Clients;

public class SentinelClient(IHttpClientFactory httpClientFactory, IAccessTokenProvider accessTokenProvider, IOptions<SentinelOptions> sentinelOptions) : ISentinelClient
{
    public async Task<Incident?> GetIncidentAsync(string id)
    {
      

        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Incident>> GetIncidentsAsync()
    {
        var requestUrl =
       $"https://management.azure.com/subscriptions/{sentinelOptions.Value.SentinelSubscriptionId}" +
       $"/resourceGroups/{sentinelOptions.Value.ResourceGroupName}" +
       "/providers/Microsoft.OperationalInsights" +
       $"/workspaces/{sentinelOptions.Value.WorkspaceName}" +
       "/providers/Microsoft.SecurityInsights/incidents" +
       "?api-version=2025-09-01";

        var httpClient = httpClientFactory.CreateClient();

        var token = await accessTokenProvider.GetManagementTokenAsync();

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.GetAsync(requestUrl);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine(content);

        return [];

    }
}