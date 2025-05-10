using FinancialTracker.Services.AuthorizeApi.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPresentationLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
