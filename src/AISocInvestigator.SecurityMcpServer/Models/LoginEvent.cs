namespace AISocInvestigator.SecurityMcpServer.Models.LoginInvestigation;

public sealed class LoginEvent
{
    public DateTimeOffset TimeGenerated { get; init; }

    public required string SourceIp { get; init; }

    public int StatusCode { get; init; }

    public required string Result { get; init; }

    public string? UserAgent { get; init; }
}