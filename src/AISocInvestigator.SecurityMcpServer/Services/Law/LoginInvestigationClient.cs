using AISocInvestigator.SecurityMcpServer.Models.LoginInvestigation;
using AISocInvestigator.SecurityMcpServer.Services.Law;
using Azure.Monitor.Query;

namespace AISocInvestigator.SecurityMcpServer.Services.LoginInvestigation;

public sealed class LoginInvestigationClient(LogsQueryClient logsQueryClient, IConfiguration configuration, ILogger<LoginInvestigationClient> logger): ILoginInvestigationClient
{
    private readonly string _workspaceId = configuration["LogAnalytics:WorkspaceId"] ?? throw new InvalidOperationException("Log Analytics WorkspaceId configuration is missing.");

    public async Task<IReadOnlyList<LoginEvent>> GetFailedLoginsAsync(
        int lastHours,
        CancellationToken cancellationToken = default)
    {
        const string query = """
            AppServiceHTTPLogs
            | where CsUriStem == "/API/login"
            | where ScStatus in (400, 401, 403)
            | project
                TimeGenerated,
                SourceIp = CIp,
                StatusCode = ScStatus,
                UserAgent
            | order by TimeGenerated desc
            """;

        try
        {
            var response = await logsQueryClient.QueryWorkspaceAsync<LoginLogRow>(
                _workspaceId,
                query,
                new QueryTimeRange(TimeSpan.FromHours(lastHours)), cancellationToken: cancellationToken);

            return response.Value
                .Select(x => new LoginEvent
                {
                    TimeGenerated = x.TimeGenerated,
                    SourceIp = x.SourceIp ?? string.Empty,
                    StatusCode = x.StatusCode,
                    Result = "Failed",
                    UserAgent = x.UserAgent
                })
                .ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve failed login events.");
            throw;
        }
    }

    public async Task<IReadOnlyList<LoginEvent>> GetSuccessfulLoginsAsync(int lastHours, CancellationToken cancellationToken = default)
    {
        const string query = """
            AppServiceHTTPLogs
            | where CsUriStem == "/API/login"
            | where ScStatus == 200
            | project
                TimeGenerated,
                SourceIp = CIp,
                StatusCode = ScStatus,
                UserAgent
            | order by TimeGenerated desc
            """;

        try
        {
            var response = await logsQueryClient.QueryWorkspaceAsync<LoginLogRow>(
                _workspaceId,
                query,
                new QueryTimeRange(TimeSpan.FromHours(lastHours)),
                cancellationToken: cancellationToken);

            return response.Value
                .Select(x => new LoginEvent
                {
                    TimeGenerated = x.TimeGenerated,
                    SourceIp = x.SourceIp ?? string.Empty,
                    StatusCode = x.StatusCode,
                    Result = "Successful",
                    UserAgent = x.UserAgent
                })
                .ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve successful login events.");
            throw;
        }
    }

    private sealed class LoginLogRow
    {
        public DateTimeOffset TimeGenerated { get; init; }

        public string? SourceIp { get; init; }

        public int StatusCode { get; init; }

        public string? UserAgent { get; init; }
    }
}