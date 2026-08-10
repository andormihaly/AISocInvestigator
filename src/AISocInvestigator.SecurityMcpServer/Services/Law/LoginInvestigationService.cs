using AISocInvestigator.SecurityMcpServer.Models.LoginInvestigation;
using AISocInvestigator.SecurityMcpServer.Services.Law;

namespace AISocInvestigator.SecurityMcpServer.Services.LoginInvestigation;

public sealed class LoginInvestigationService(
    ILoginInvestigationClient loginInvestigationClient)
    : ILoginInvestigationService
{
    public Task<IReadOnlyList<LoginEvent>> GetFailedLoginsAsync(int lastHours, CancellationToken cancellationToken = default)
    {
        ValidateLastHours(lastHours);

        return loginInvestigationClient.GetFailedLoginsAsync(lastHours,cancellationToken);
    }

    public Task<IReadOnlyList<LoginEvent>> GetSuccessfulLoginsAsync(int lastHours, CancellationToken cancellationToken = default)
    {
        ValidateLastHours(lastHours);

        return loginInvestigationClient.GetSuccessfulLoginsAsync(lastHours, cancellationToken);
    }

    private static void ValidateLastHours(int lastHours)
    {
        if (lastHours <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lastHours), "LastHours must be greater than zero.");
        }
    }
}