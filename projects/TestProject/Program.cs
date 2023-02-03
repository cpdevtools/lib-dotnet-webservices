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
builder.Services.SetupWebserviceHealthCheck();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseWebserviceDocumentation();

app.UseCookiePolicy();
app.UseWebserviceForwardedHeaders();

app.UseWebserviceCors();
app.UseWebserviceJwtAuthentication();
app.UseWebserviceExceptionHandlers();
app.UseWebserviceMvc();
app.UseWebserviceHealthCheck();

app.MigrateDatabases();
app.RunWebservice();
