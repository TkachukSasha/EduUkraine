using Core.Entities;
using Core.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints;

public class GetUniversititesEndpoint(IUniversityService universityService) : Endpoint<GetUniversitiesQuery, List<University>>
{
    private readonly IUniversityService _universityService = universityService;

    public override void Configure()
    {
        Get("api/universities");
    }

    public override async Task<Ok<List<University>>> HandleAsync(GetUniversitiesQuery req, CancellationToken ct) =>
        TypedResults.Ok(await _universityService.GetUniversitiesAsync(req, ct));
}
