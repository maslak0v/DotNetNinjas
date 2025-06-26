using FinancialTracker.Services.Analytics;
using FinancialTracker.Services.Analytics.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallDbConnection();

builder.Services.InstallAutoMapper();
builder.Services.InstallServices();
builder.Services.InstallAdviceService();
builder.Services.InstallRepositories();
    
builder.Services.AddControllers();
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

app.UseHttpsRedirection();
app.MapControllers();
app.Run();