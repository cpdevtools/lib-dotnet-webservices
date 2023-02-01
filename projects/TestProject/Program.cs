using System.Reflection;
using CpDevTools.Webservices.Extensions;

var builder = WebApplication.CreateBuilder(args);

var logger = builder.SetupWebserviceLogging();

builder.Configuration.SetupDockerWebserviceConfiguration();

builder.Services.SetupWebserviceDocumentation();
builder.Services.SetupWebserviceJwtAuthentication();
builder.Services.SetupWebserviceCors();
builder.Services.SetupWebserviceMvc();
builder.Services.SetupWebserviceSameSiteCookies();
builder.Services.SetupWebserviceExceptionHandlers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseWebserviceDocumentation();
app.UseWebserviceSameSiteCookies();
app.UseWebserviceCors();
app.UseWebserviceJwtAuthentication();
app.UseWebserviceExceptionHandlers();
app.UseWebserviceMvc();


app.RunWebservice();

Console.WriteLine();