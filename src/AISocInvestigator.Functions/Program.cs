using AISocInvestigator.Functions.Authentication;
using AISocInvestigator.Functions.Clients;
using AISocInvestigator.Functions.Configuration;
using AISocInvestigator.Functions.Services;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.ResourceManager;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AISocInvestigator.Functions 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    var config = configuration.Build();

                    var keyVaultUri = "https://andorskv.vault.azure.net/";

                    configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                    services.AddHttpClient();
                    services.AddSingleton<IIncidentService, IncidentService>();
                    services.AddSingleton<ISentinelClient, SentinelClient>();
                    services.Configure<SentinelOptions>(context.Configuration.GetSection("Sentinel"));
                    services.AddSingleton(new ArmClient(new DefaultAzureCredential()));

                })
                .Build();

            host.Run();
        }
    }
}