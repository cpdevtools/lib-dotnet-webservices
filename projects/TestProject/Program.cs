using CpDevTools.Webservices.Extensions;

var builder = WebApplication.CreateBuilder(args)!;

var logger = builder.SetupWebserviceLogging();

builder.Configuration.SetupDockerWebserviceConfiguration();

builder.Services.SetupWebserviceDocumentation();
builder.Services.SetupWebserviceCors();
builder.Services.SetupWebserviceJwtAuthentication();
builder.Services.SetupWebserviceControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseWebserviceDocumentation();
app.UseWebserviceCors();
app.UseWebserviceJwtAuthentication();
app.UseWebserviceControllers();

app.Run();
