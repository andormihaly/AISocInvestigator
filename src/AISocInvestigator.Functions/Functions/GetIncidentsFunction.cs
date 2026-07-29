using AISocInvestigator.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace AISocInvestigator.Functions.Functions;

public class GetIncidentsFunction(IIncidentService incidentService)
{
    [Function(nameof(GetIncidentsFunction))]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "incidents")] HttpRequestData request)
    {
        var incidents = await incidentService.GetIncidentsAsync();

        var response = request.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(incidents);

        return response;
    }
}