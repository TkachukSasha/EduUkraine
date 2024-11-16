using Core.Entities;
using Core.Sparql;
using MongoDB.Driver;
using Quartz;

namespace Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class ProcessUniversitiesJob(
    ISparqlService sparqlService,
    IUniversityRepository universityRepository) : IJob
{
    private readonly ISparqlService _sparqlService = sparqlService;
    private readonly IUniversityRepository _universityRepository = universityRepository;

    public async Task Execute(IJobExecutionContext context)
    {
        List<University> items = await _sparqlService.GetUniversitiesAsync(context.CancellationToken);

        if (!items.Any())
        {
            return;
        }

        List<University> universities = await _universityRepository
             .Universities
             .Find(_ => true)
             .ToListAsync(context.CancellationToken);

        var universitiesBatch = items.Where(item =>
          !universities.Any(existing =>
              existing.InstitutionLabel == item.InstitutionLabel &&
              existing.CityLabel == item.CityLabel &&
              existing.FoundationDate == item.FoundationDate))
          .ToList();

        if (universitiesBatch.Any())
        {
            await _universityRepository
                .Universities
                .InsertManyAsync(universitiesBatch);
        }
    }
}
