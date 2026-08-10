using AISocInvestigator.SecurityMcpServer.Configuration;
using AISocInvestigator.SecurityMcpServer.Services.Sentinel;
using AISocInvestigator.SecurityMcpServer.Services.ServerInfo;
using Azure.Identity;
using Azure.ResourceManager;

namespace AISocInvestigator.SecurityMcpServer.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SentinelOptions>(configuration.GetSection(SentinelOptions.SectionName));

        services.AddSingleton<IServerInfoService, ServerInfoService>();
        services.AddSingleton<ISentinelClient, SentinelClient>();
        services.AddSingleton<ISentinelService, SentinelService>();

        return services;
    }
}