using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FinancialTracker.Frontend;
using FinancialTracker.Frontend.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<HttpClientFactory>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<BrowserStorage>();

builder.Services.AddMudServices();

var app = builder.Build();
await app.RunAsync();