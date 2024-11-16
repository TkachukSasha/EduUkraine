using Core.Entities;
using MongoDB.Driver;

namespace Infrastructure.Database;

public sealed class MongoDbContext(IMongoClient mongoClient) : IUniversityRepository
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase(MongoDbConsts.DatabaseName);

    public IMongoCollection<University> Universities => _database.GetCollection<University>(MongoDbConsts.UniversitiesCollection);
}
