using KarizmaConnection.Test.Server.Services;
using KarizmaPlatform.Connection.Server.Config;
using KarizmaPlatform.Connection.Server.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKarizmaConnection(new Options
{
    DefaultHubResponseErrorCode = 500,
    ReturnStackTraceOnError = false
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "v1" });
    options.AddKarizmaConnectionSwaggerDocs();
});

builder.Services.AddControllers();
builder.Services.AddTransient<TestService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapKarizmaConnectionHub("/Hub");
app.Run();