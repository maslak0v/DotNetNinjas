using FinancialTracker.Services.Analytics;
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

builder.Services.AddControllers();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigrationsAsync();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();