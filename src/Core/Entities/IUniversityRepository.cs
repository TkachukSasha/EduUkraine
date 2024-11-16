using MongoDB.Driver;

namespace Core.Entities;

public interface IUniversityRepository
{
    IMongoCollection<University> Universities { get; }
}
