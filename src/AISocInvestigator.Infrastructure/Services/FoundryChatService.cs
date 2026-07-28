using AISocInvestigator.Application.Configuration;
using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Interfaces;
using AISocInvestigator.Application.Models;
using Azure.AI.OpenAI;
using Azure.AI.Projects;
using Microsoft.Extensions.Options;

namespace AISocInvestigator.Infrastructure.Services;

public sealed class FoundryChatService(AIProjectClient aiProjectClient, IOptions<FoundryOptions> options) : IAIChatService
{
    public async Task<Result<ChatResponse>> AskAsync(string message, CancellationToken cancellationToken = default)
    {
        try
        {

            //var chatClient = openAIClient.GetChatClient(options.Value.DeploymentName);
            //var response = await chatClient.CompleteChatAsync(message);
            //return Result<ChatResponse>.Success(new ChatResponse(response.Value.Content[0].Text));

            var responsesClient = aiProjectClient.ProjectOpenAIClient.GetProjectResponsesClientForModel(options.Value.DeploymentName);
            var response = await responsesClient.CreateResponseAsync(message, cancellationToken: cancellationToken);

            return Result<ChatResponse>.Success(new ChatResponse(response.Value.GetOutputText()));

        }
        catch (Exception exception)
        {
            return Result<ChatResponse>.Failure(exception.Message);
        }

    }
}