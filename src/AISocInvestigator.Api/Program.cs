using AISocInvestigator.Application.Configuration;
using AISocInvestigator.Application.Features.Chat;
using AISocInvestigator.Application.Interfaces;
using AISocInvestigator.Infrastructure.Services;
using Azure.AI.OpenAI;
using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Responses;
using Scalar.AspNetCore;
using System.ClientModel.Primitives;
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


builder.Services.AddSingleton<AzureOpenAIClient>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<FoundryOptions>>().Value;
    return new AzureOpenAIClient(new Uri(options.FoundryEndpoint), new DefaultAzureCredential());
});

builder.Services.AddSingleton<AIProjectClient>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<FoundryOptions>>();
    return new AIProjectClient(new Uri(options.Value.ProjectEndpoint), credential);
});

builder.Services.AddScoped<IAIChatService, FoundryChatService>();

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