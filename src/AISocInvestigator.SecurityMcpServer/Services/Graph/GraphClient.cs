using AISocInvestigator.SecurityMcpServer.Models;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models.Security;

namespace AISocInvestigator.SecurityMcpServer.Services.Graph;

public sealed class GraphClient(GraphServiceClient graphServiceClient) : IGraphClient
{
    public async Task<DefenderAlert> CreateAlertAsync(CreateAlertRequest request)
    {
        var requestBody = new ManualAlert
        {
            OdataType = "#microsoft.graph.security.manualAlert",
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Severity = Enum.Parse<AlertSeverity>(request.Severity, true),
            EntityDefinitions =
            [
                new EntityDefinitionInput
                {
                    EntityType = Enum.Parse<ManualAlertEntityType>(request.EntityType, true),
                    EntityIdentifier = request.EntityIdentifier,
                    IdentifierValue = request.EntityValue,
                    Role = EntityDefinitionInputRole.Impacted
                }
            ]
        };

        var alert = await graphServiceClient.Security.Alerts_v2.PostAsync(requestBody)
            ?? throw new InvalidOperationException("Microsoft Graph did not return the created alert.");

        return new DefenderAlert(
            alert.Id ?? throw new InvalidOperationException("Created alert does not contain an ID."),
            alert.Title ?? request.Title,
            alert.Severity?.ToString() ?? request.Severity,
            alert.Status?.ToString() ?? "Unknown",
            alert.Category,
            alert.CreatedDateTime,
            alert.AlertWebUrl);
    }
    public async Task<IReadOnlyList<DefenderAlert>> GetAlertsAsync(DateTimeOffset? from = null)
    {
        var response = await graphServiceClient.Security.Alerts_v2.GetAsync(requestConfiguration =>
        {
            if (from.HasValue)
            {
                requestConfiguration.QueryParameters.Filter = $"createdDateTime ge {from.Value.UtcDateTime:yyyy-MM-ddTHH:mm:ssZ}";
            }
        });

        if (response?.Value is null)
        {
            return [];
        }

        return response.Value.Select(alert => new DefenderAlert(
            alert.Id ?? string.Empty,
            alert.Title ?? string.Empty,
            alert.Severity?.ToString() ?? "Unknown",
            alert.Status?.ToString() ?? "Unknown",
            alert.Category,
            alert.CreatedDateTime,
            alert.AlertWebUrl)).ToList();
    }
    public async Task<DefenderAlert?> GetAlertAsync(string alertId)
    {
        var alert = await graphServiceClient.Security.Alerts_v2[alertId].GetAsync();

        if (alert is null)
        {
            return null;
        }

        return new DefenderAlert(
            alert.Id ?? string.Empty,
            alert.Title ?? string.Empty,
            alert.Severity?.ToString() ?? "Unknown",
            alert.Status?.ToString() ?? "Unknown",
            alert.Category,
            alert.CreatedDateTime,
            alert.AlertWebUrl);
    }
}