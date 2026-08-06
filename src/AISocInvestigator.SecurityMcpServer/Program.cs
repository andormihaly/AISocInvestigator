using ModelContextProtocol.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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