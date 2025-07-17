using FinancialTracker.Services.Analytics;
using System.Text.Json;
using WebInfrastructure.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallDbConnection();

builder.Services.InstallAutoMapper();
builder.Services.InstallServices();
builder.Services.InstallAdviceService();
builder.Services.InstallRepositories();
builder.Services.AddMessaging();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
}); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.ApplyMigrationsAsync();
}

app.UsegGlobalExceptionMiddleware();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();