using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using Infrastructure.Database;
using Core.Entities;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using VDS.RDF.Query;
using Core.Sparql;
using Infrastructure.Sparql;

namespace Infrastructure;

public static class DependencyInjection
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Ликвидировать объекты перед потерей области", Justification = "<Ожидание>")]
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string? mongoConnectionString = configuration.GetConnectionString("MongoDb");
        var mongoClientSettings = MongoClientSettings.FromConnectionString(mongoConnectionString);

        services.AddSingleton<IMongoClient>(new MongoClient(mongoClientSettings));

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        ConventionRegistry.Register("camelCase", new ConventionPack {
            new CamelCaseElementNameConvention()
        }, _ => true);

        services.TryAddSingleton<IUniversityRepository, MongoDbContext>();
        services.TryAddSingleton<MongoDbContext>();

        services.AddHttpClient<SparqlQueryClient>(client =>
        {
            var sparqlUri = new Uri(configuration.GetConnectionString("Sparql") ?? string.Empty);
            client.BaseAddress = sparqlUri;
        });

        services.AddSingleton<ISparqlService, SparqlService>();

        services.AddQuartz();

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}
