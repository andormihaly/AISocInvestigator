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
            alert.AlertWebUrl);
    }
}