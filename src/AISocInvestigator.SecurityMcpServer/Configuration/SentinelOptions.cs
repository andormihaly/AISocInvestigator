namespace AISocInvestigator.SecurityMcpServer.Configuration;

public sealed class SentinelOptions
{
    public const string SectionName = "Sentinel";

    public required string SentinelSubscriptionId { get; init; }

    public required string ResourceGroupName { get; init; }

    public required string WorkspaceName { get; init; }
}