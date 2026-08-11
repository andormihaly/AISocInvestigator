using AISocInvestigator.Application.Configuration;
using AISocInvestigator.Application.Features.Chat;
using AISocInvestigator.Application.Interfaces;
using AISocInvestigator.Infrastructure.AgentFramework;
using AISocInvestigator.Infrastructure.Services;
using AISocInvestigator.Infrastructure.Telemetry;
using AISocInvestigator.Infrastructure.Workflows;
using AISocInvestigator.Infrastructure.Workflows.Executors;
using AISocInvestigator.Infrastructure.Workflows.Sessions;
using Azure.AI.Projects;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.Options;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);



builder.Host.UseWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(AskChatHandler).Assembly);
});

var keyVaultUri = builder.Configuration["KeyVaultUri"] ?? throw new InvalidOperationException("KeyVaultUri configuration is missing.");

var credential = builder.Environment.IsDevelopment() ? new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeManagedIdentityCredential = true }) : new DefaultAzureCredential();
builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), credential);


builder.Services.Configure<FoundryOptions>(builder.Configuration.GetSection("Foundry"));

builder.Services.AddSingleton<AIProjectClient>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<FoundryOptions>>();
    return new AIProjectClient(new Uri(options.Value.ProjectEndpoint), credential);
});

builder.Services.ConfigureOpenTelemetryTracerProvider((serviceProvider, tracing) => tracing.AddSource(TelemetryConstants.ActivitySourceName));

builder.Services.AddOpenTelemetry().UseAzureMonitor(options =>
{
    options.ConnectionString = builder.Configuration["AzureMonitor:ConnectionString"] ?? throw new InvalidOperationException("Azure Monitor connection string is missing.");
});

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IWorkflowSessionManager, WorkflowSessionManager>();
builder.Services.AddSingleton<IWorkflowSessionFactory, WorkflowSessionFactory>();
builder.Services.AddScoped<IAIChatService, FoundryChatService>();
builder.Services.AddScoped<IAIAgentService, SocAgentService>();
builder.Services.AddSingleton<IAgentFactory, FoundryAgentFactory>();

builder.Services.AddSingleton<IntakeExecutor>();
builder.Services.AddSingleton<InvestigatorExecutor>();
builder.Services.AddSingleton<KnowledgeExecutor>();
builder.Services.AddSingleton<InvestigatorMCPExecutor>();
builder.Services.AddSingleton<SocWorkflowDefinition>();
builder.Services.AddSingleton<IWorkflowRunner, WorkflowRunner>();
builder.Services.AddSingleton<ISocWorkflow, SocWorkflow>();

builder.Services.AddOpenApi();

builder.Services.AddControllers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();