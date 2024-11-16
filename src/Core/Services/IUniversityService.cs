using System.Globalization;
using Core.Entities;
using MongoDB.Driver;

namespace Core.Services;

public record GetUniversitiesQuery(string? InstitutionLabel, string? CityLabel, DateOnly? FoundationDate, string? SortDirection = "asc", int Page = 0, int PageSize = 100);

public interface IUniversityService
{
    Task<List<University>> GetUniversitiesAsync(GetUniversitiesQuery query, CancellationToken cancellationToken);
}

public sealed class UniversityService(IUniversityRepository universityRepository) : IUniversityService
{
    private readonly IUniversityRepository _universityRepository = universityRepository;

    public async Task<List<University>> GetUniversitiesAsync(GetUniversitiesQuery query, CancellationToken cancellationToken)
    {
        FilterDefinition<University> filter = Builders<University>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(query.InstitutionLabel))
        {
            filter &= Builders<University>.Filter.Eq(u => u.InstitutionLabel, query.InstitutionLabel);
        }

        if (!string.IsNullOrWhiteSpace(query.CityLabel))
        {
            filter &= Builders<University>.Filter.Eq(u => u.CityLabel, query.CityLabel);
        }

        if (query.FoundationDate.HasValue)
        {
            filter &= Builders<University>.Filter.Eq(u => u.FoundationDate, query.FoundationDate);
        }

        SortDefinition<University> sortDefinition = query.SortDirection?.ToLower(CultureInfo.CurrentCulture) switch
        {
            "asc" => Builders<University>.Sort.Ascending(query.SortDirection),
            "desc" => Builders<University>.Sort.Descending(query.SortDirection),
            _ => Builders<University>.Sort.Ascending(query.SortDirection)
        };

        int skip = query.Page * query.PageSize;
        int take = query.PageSize;

        List<University> universities = await _universityRepository
            .Universities
            .Find(filter)
            .Sort(sortDefinition)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(cancellationToken);

        return universities;
    }
}
