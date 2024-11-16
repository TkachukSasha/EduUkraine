using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace Integration.Tests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .Build();

    public HttpClient? Client { get; private set; }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            ServiceDescriptor? mongoSettingsDescriptor = services
                .SingleOrDefault(x => x.ServiceType == typeof(MongoClientSettings));

            ServiceDescriptor? mongoClientDescriptor = services
                .SingleOrDefault(x => x.ServiceType == typeof(IMongoClient));

            if(mongoSettingsDescriptor is not null)
            {
                services.Remove(mongoSettingsDescriptor);
            }

            if(mongoClientDescriptor is not null)
            {
                services.Remove(mongoClientDescriptor);
            }

            var mongoClientSettings = MongoClientSettings.FromConnectionString(_mongoDbContainer.GetConnectionString());

            services.AddSingleton<IMongoClient>(new MongoClient(mongoClientSettings));
        });
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();

        Client = CreateClient();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
       await _mongoDbContainer.DisposeAsync();
        Client?.Dispose();
    }
}
