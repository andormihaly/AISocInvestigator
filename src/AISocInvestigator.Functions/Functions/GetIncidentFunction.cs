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
        var incident = await incidentService.GetIncidentAsync(id);

        if (incident is null)
        {
            return request.CreateResponse(HttpStatusCode.NotFound);
        }

        var response = request.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(incident);

        return response;
    }
}