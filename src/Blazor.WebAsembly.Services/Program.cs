using Blazor.WebAsembly.Services.IdentityData;
using Microsoft.EntityFrameworkCore;
using Blazor.WebAsembly.Services.Configurations;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.RegisterServices();

builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApiConfiguration();
app.UseSwaggerConfiguration();

app.Run();
