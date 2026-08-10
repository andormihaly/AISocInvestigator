using AISocInvestigator.SecurityMcpServer.Extensions;
using AISocInvestigator.SecurityMcpServer.Services.Law;
using AISocInvestigator.SecurityMcpServer.Services.LoginInvestigation;
using Azure.Identity;
using Azure.Monitor.Query;
using Azure.ResourceManager;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUri = builder.Configuration["KeyVaultUri"] ?? throw new InvalidOperationException("KeyVaultUri configuration is missing.");

var credential = builder.Environment.IsDevelopment()
    ? new DefaultAzureCredential(
        new DefaultAzureCredentialOptions
        {
            ExcludeManagedIdentityCredential = true
        })
    : new DefaultAzureCredential();

builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), credential);

builder.Services.AddControllers();

builder.Services.AddSingleton(sp => new ArmClient(credential));
builder.Services.AddSingleton(new LogsQueryClient(credential));
builder.Services.AddSingleton<ILoginInvestigationClient,LoginInvestigationClient>();
builder.Services.AddSingleton< ILoginInvestigationService,LoginInvestigationService>();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

app.MapControllers();

// Health endpoint
app.MapGet("/", () => "AI SOC Investigator Security MCP Server");

// MCP endpoint
app.MapMcp("/mcp");

app.Run();