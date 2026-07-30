using AISocInvestigator.Application.Configuration;
using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Interfaces;
using AISocInvestigator.Application.Models;
using AISocInvestigator.Infrastructure.Prompts;
using Azure.AI.Projects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#pragma warning disable OPENAI001
namespace AISocInvestigator.Infrastructure.Services;

public sealed class FoundryChatService(AIProjectClient aiProjectClient, IOptions<FoundryOptions> options, ILogger<FoundryChatService> logger) : IAIChatService
{
    public async Task<Result<ChatResponse>> AskAsync(string message, string? previousResponseId, CancellationToken cancellationToken = default)
    {
        try
        {
            var responsesClient = aiProjectClient.ProjectOpenAIClient.GetProjectResponsesClientForModel(options.Value.DeploymentName);

            var requestOptions = new OpenAI.Responses.CreateResponseOptions
            {
                Instructions = SocInvestigatorPrompts.DefaultSystemInstructions,
                PreviousResponseId = previousResponseId
            };

            requestOptions.InputItems.Add(OpenAI.Responses.ResponseItem.CreateUserMessageItem(message));

            var response = await responsesClient.CreateResponseAsync(requestOptions, cancellationToken);

            return Result<ChatResponse>.Success(new ChatResponse(response.Value.GetOutputText(), response.Value.Id));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to generate a response using Microsoft Foundry.");
            return Result<ChatResponse>.Failure("The AI service could not process the request.");
        }

    }
}
#pragma warning restore OPENAI001