using AISocInvestigator.SecurityMcpServer.Models.LoginInvestigation;

namespace AISocInvestigator.SecurityMcpServer.Services.Law
{
    public interface ILoginInvestigationClient
    {
        Task<IReadOnlyList<LoginEvent>> GetFailedLoginsAsync(int lastHours, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<LoginEvent>> GetSuccessfulLoginsAsync(int lastHours, CancellationToken cancellationToken = default);
    }
}
