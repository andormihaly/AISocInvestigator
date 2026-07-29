using Azure.Core;
using Azure.Identity;

namespace AISocInvestigator.Functions.Authentication;

public class AccessTokenProvider : IAccessTokenProvider
{
    private readonly DefaultAzureCredential _credential = new();

    public async Task<string> GetManagementTokenAsync(CancellationToken cancellationToken = default)
    {
        var context = new TokenRequestContext(
        [
            "https://management.azure.com/.default"
        ]);

        var token = await _credential.GetTokenAsync(context, cancellationToken);

        return token.Token;
    }
}