namespace Core.Entities;

public sealed class University
{
    public Guid Id { get; set; }

    public required string Institution { get; set; }

    public required string InstitutionLabel { get; set; }

    public required string City { get; set; }

    public required string CityLabel { get; set; }

    public required DateOnly FoundationDate { get; set; }

    public string WebSite { get; set; } = string.Empty;

    public string Logo { get; set; } = string.Empty;
}
