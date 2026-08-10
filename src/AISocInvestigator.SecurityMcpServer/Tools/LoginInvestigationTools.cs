using AISocInvestigator.SecurityMcpServer.Models.LoginInvestigation;
using AISocInvestigator.SecurityMcpServer.Services.Law;
using AISocInvestigator.SecurityMcpServer.Services.LoginInvestigation;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AISocInvestigator.SecurityMcpServer.Tools.LoginInvestigation;

[McpServerToolType]
public sealed class LoginInvestigationTools(ILoginInvestigationService loginInvestigationService)
{
    [McpServerTool]
    [Description("Returns failed application login attempts from the specified number of previous hours.")]
    public Task<IReadOnlyList<LoginEvent>> GetFailedLogins([Description("Number of hours to look back.")]int lastHours, CancellationToken cancellationToken)
    {
        return loginInvestigationService.GetFailedLoginsAsync(lastHours,cancellationToken);
    }

    [McpServerTool]
    [Description("Returns successful application logins from the specified number of previous hours.")]
    public Task<IReadOnlyList<LoginEvent>> GetSuccessfulLogins([Description("Number of hours to look back.")]int lastHours, CancellationToken cancellationToken)
    {
        return loginInvestigationService.GetSuccessfulLoginsAsync(lastHours, cancellationToken);
    }
}