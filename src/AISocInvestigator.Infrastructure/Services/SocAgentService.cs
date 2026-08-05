using AISocInvestigator.Application.Configuration;
using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Interfaces;
using AISocInvestigator.Application.Models;
using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Responses;

#pragma warning disable OPENAI001

namespace AISocInvestigator.Infrastructure.Services;

public sealed class SocAgentService(AIProjectClient aiProjectClient, IOptions<FoundryOptions> options, ILogger<SocAgentService> logger) : IAIAgentService
{
    public async Task<Result<ChatResponse>> ExecuteAsync(string message, string? previousResponseId, CancellationToken cancellationToken = default)
    {
        try
        {
            var agentReference = new AgentReference(name: options.Value.InvestigatorAgentName);
            var responsesClient = aiProjectClient.ProjectOpenAIClient.GetProjectResponsesClientForAgent(agentReference);

            var requestOptions = new CreateResponseOptions
            {
                PreviousResponseId = previousResponseId
            };

            requestOptions.InputItems.Add(ResponseItem.CreateUserMessageItem(message));

            var response = await responsesClient.CreateResponseAsync(requestOptions, cancellationToken);

            return Result<ChatResponse>.Success(new ChatResponse(response.Value.GetOutputText(), response.Value.Id));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to generate a response using the SOC Investigator agent.");
            return Result<ChatResponse>.Failure("The AI agent could not process the request.");
        }
    }
}

#pragma warning restore OPENAI001