using Blazor.WebAssembly.UI;
using MatBlazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.Configuration.SetBasePath(builder.HostEnvironment.BaseAddress)
//    .AddJsonFile("appsettings.json", true, true)
//    .AddJsonFile($"appsettings.{builder.HostEnvironment.Environment}.json", true, true);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var apiBaseUrl = builder.Configuration.GetSection("ApiBaseUrl").Value;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddMatBlazor();

await builder.Build().RunAsync();
