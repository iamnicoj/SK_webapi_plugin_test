using System.IO;
using Chatbot.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Core;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the connection string
string connectionString = builder.Configuration.GetConnectionString("AppConfig");

// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(connectionString);

// Bind configuration "chatbot:Settings" section to the Settings object
builder.Services.Configure<AzureOpenAIOptions>(builder.Configuration.GetSection("chatbot:Settings"));

// Add services to the container.
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get configuration
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddUserSecrets<Program>()
    .Build();

// Set open AI options
var AOAiIOptions = builder.Configuration.GetSection("chatbot:Settings").Get<AzureOpenAIOptions>();

// Add Semantic Kernel
var kernelBuilder = builder.Services.AddKernel();
kernelBuilder.Plugins.AddFromType<TimePlugin>();

builder.Services.AddAzureOpenAIChatCompletion(
    AOAiIOptions.ChatDeploymentName,
    AOAiIOptions.Endpoint,
    AOAiIOptions.ApiKey);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
