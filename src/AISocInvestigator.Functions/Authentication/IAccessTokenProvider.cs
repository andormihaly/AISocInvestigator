namespace AISocInvestigator.Functions.Authentication;

public interface IAccessTokenProvider
{
    Task<string> GetManagementTokenAsync(CancellationToken cancellationToken = default);
}