using Api.Middlewares;
using Infrastructure;
using Microsoft.Extensions.Options;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) 
    => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.Configure<AuthConfig>(builder.Configuration.GetSection("Auth"));
builder.Services.AddSingleton(registeredServices =>
    registeredServices.GetRequiredService<IOptions<AuthConfig>>()!.Value);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyAuthorizationMiddleware>();

app.Run();

public partial class Program;
