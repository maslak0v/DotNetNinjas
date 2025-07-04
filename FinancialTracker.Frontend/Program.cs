using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FinancialTracker.Frontend;
using FinancialTracker.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("http://localhost:5010/") });

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BrowserStorage>();

builder.Services.AddSingleton(builder.Configuration);

await builder.Build().RunAsync();