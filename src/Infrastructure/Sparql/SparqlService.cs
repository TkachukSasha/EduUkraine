using Core.Sparql;
using Core.Entities;
using VDS.RDF.Query;

namespace Infrastructure.Sparql;

public sealed class SparqlService(SparqlQueryClient sparqlClient) : ISparqlService
{
    private const string SparqlQuery = """
        SELECT ?institution ?institutionLabel ?city ?cityLabel
               ?foundationDate ?website ?logo
        WHERE {
          ?institution wdt:P31 wd:Q3918;            # Об'єкти, які є навчальними закладами
                      wdt:P17 wd:Q212;            # Знаходяться в Україні
                      wdt:P131 ?city.            # Місто, в якому знаходиться навчальний заклад
          OPTIONAL { ?institution wdt:P279 ?type. } # Тип навчального закладу (наприклад, університет, технікум)
          OPTIONAL { ?institution wdt:P571 ?foundationDate. } # Дата заснування
          OPTIONAL { ?institution wdt:P856 ?website. } # Вебсайт
          OPTIONAL { ?institution wdt:P154 ?logo. } # Логотип

          SERVICE wikibase:label { bd:serviceParam wikibase:language "[AUTO_LANGUAGE],en". }
        }
        """;

    private readonly SparqlQueryClient _sparqlClient = sparqlClient;

    public async Task<List<University>> GetUniversitiesAsync(CancellationToken cancellationToken)
    {
        SparqlResultSet? resultSet = await _sparqlClient.QueryWithResultSetAsync(SparqlQuery, cancellationToken);

        return resultSet?
           .Results
           .Select(UniversityMappingExtensions.Map)
           .ToList() ?? new List<University>();
    }
}
