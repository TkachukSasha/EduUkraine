using Core.Entities;

namespace Core.Sparql;

public interface ISparqlService
{
    Task<List<University>> GetUniversitiesAsync(CancellationToken cancellationToken);
}
