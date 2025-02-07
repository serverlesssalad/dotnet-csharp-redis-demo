using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;

using DotNetEnv;

Env.Load(); // Loads .env file if present

// Set the options with the desired content root path
var options = new WebApplicationOptions
{
    ContentRootPath = "/"
};

var builder = WebApplication.CreateBuilder(options);

// Load Redis configuration
var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "localhost";
var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379";
var redisUsername = Environment.GetEnvironmentVariable("REDIS_USERNAME") ?? "default";
var redisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD") ?? "";

var redisConnectionString = $"{redisHost}:{redisPort},password={redisPassword},ssl=False";

Console.WriteLine($"Connecting to Redis at {redisHost}:{redisPort}");

// Register Redis connection
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    return ConnectionMultiplexer.Connect(redisConnectionString);
});

builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Map the /health endpoint
app.MapGet("/health", () =>
{
    try
    {
        var redisConnection = app.Services.GetRequiredService<IConnectionMultiplexer>();
        if (redisConnection.IsConnected)
        {
            return Results.Ok(new { status = "healthy" });
        }
        return Results.Json(new { status = "unhealthy", message = "Unable to connect to Redis" }, statusCode: 500);
    }
    catch (Exception ex)
    {
        return Results.Json(new { status = "unhealthy", message = ex.Message }, statusCode: 500);
    }
});

// Map the root ("/") endpoint
app.MapGet("/", () => Results.Ok("Welcome to the application!"));

// app.UseHttpsRedirection();
app.MapControllers();

app.Run();

