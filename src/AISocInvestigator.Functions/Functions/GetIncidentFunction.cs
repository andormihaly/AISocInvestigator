using AISocInvestigator.Domain.Incidents;
using AISocInvestigator.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace AISocInvestigator.Functions.Functions;

public class GetIncidentFunction(IIncidentService incidentService)
{
    [Function(nameof(GetIncidentFunction))]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function,"get", Route = "incidents/{id}")] HttpRequestData request, string id)
    {
        var incident = new Incident
        {
            Id = id,
            Title = "Multiple failed login attempts in app",
            Severity = "High",
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            User = "john.doe@contoso.com",
            SourceIp = "192.168.1.100",
            Description = "Five failed login attempts detected within five minutes."
        };

        var response = request.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(incident);

        return response;
    }
}