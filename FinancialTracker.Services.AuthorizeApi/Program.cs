using FinancialTracker.Services.AuthorizeApi.Infrastructure.DIInfrastructure;
using FinancialTracker.Services.AuthorizeApi.Presentation;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

//Add dependencies from layers
builder.Services.AddInfrastructureLayer(configuration);
builder.Services.AddPresentationLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
