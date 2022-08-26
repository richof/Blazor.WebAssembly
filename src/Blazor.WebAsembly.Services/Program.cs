using Blazor.WebAsembly.Services.AutoMapper;
using Blazor.WebAsembly.Services.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region business services

builder.Services.AddScoped<IDataAccess, DataAccess>();
builder.Services.AddScoped<ICompanyData, CompanyData>();
#endregion
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    // .AllowAnyOrigin()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials()); app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
