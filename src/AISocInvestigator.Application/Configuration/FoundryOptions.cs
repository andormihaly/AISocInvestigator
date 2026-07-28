namespace AISocInvestigator.Application.Configuration;

public sealed class FoundryOptions
{
    public string OpenAIEndpoint { get; set; } = string.Empty;
    public string DeploymentName { get; set; } = string.Empty;
    public string ProjectEndpoint { get; set; } = string.Empty;
    public string FoundryEndpoint { get; set; } = string.Empty;
}