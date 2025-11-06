using FinancialTracker.Services.Analytics;
using System.Text.Json;
using WebInfrastructure.Shared.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

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
builder.Services.AddSwaggerGen( options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Документация Analytics API",
        Version = "v1",
        Description = "Описание API сервиса аналитики."
    });
    options.TagActionsBy(api => new[] { api.GroupName });
    options.DocInclusionPredicate((version, desc) => true);
});

builder.Services.AddCors(options => 
    options.AddPolicy("AllowAllOrigins", builder => 
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigrationsAsync();
}

app.UsegGlobalExceptionMiddleware();

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();