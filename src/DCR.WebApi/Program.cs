using DCR.WebApi.Scope.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomControllers();
builder.Services.AddCustomSwagger();
builder.Services.AddCustomServices();
builder.Services.AddCustomCache(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.MapControllers();

app.Run();
