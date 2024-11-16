using System.Globalization;
using VDS.RDF.Query;

namespace Core.Entities;

public static class UniversityMappingExtensions
{
    private static readonly string[] CultureFormats = ["yyyy-MM-dd", "yyyy/MM/dd", "dd/MM/yyyy"];

    public static University Map(ISparqlResult result)
    {
        string institution = result["institution"]?.ToString() ?? string.Empty;
        string institutionLabel = result["institutionLabel"]?.ToString() ?? string.Empty;
        string city = result["city"]?.ToString() ?? string.Empty;
        string cityLabel = result["cityLabel"]?.ToString() ?? string.Empty;
        string? foundationDate = result["foundationDate"]?.ToString();
        string? website = result["website"]?.ToString();
        string? logo = result["logo"]?.ToString();

        DateOnly parsedFoundationDate = foundationDate.ParseFoundationDate();

        return new University
        {
            Institution = institution,
            InstitutionLabel = institutionLabel,
            City = city,
            CityLabel = cityLabel,
            FoundationDate = parsedFoundationDate,
            WebSite = website ?? string.Empty,
            Logo = logo.ConvertToBase64() ?? string.Empty
        };
    }

    private static DateOnly ParseFoundationDate(this string? foundationDate)
    {
        if (string.IsNullOrEmpty(foundationDate))
        {
            return DateOnly.MinValue;
        }
        CultureInfo? cultureInfo = CultureInfo.InvariantCulture;

        if (DateTime.TryParseExact(foundationDate, CultureFormats, cultureInfo, System.Globalization.DateTimeStyles.None, out DateTime date))
        {
            return DateOnly.FromDateTime(date);
        }

        return DateOnly.MinValue;
    }

    private static string ConvertToBase64(this string? logo)
    {
        if (string.IsNullOrWhiteSpace(logo))
        {
            return string.Empty;
        }

        byte[] imageBytes = File.ReadAllBytes(logo);
        return Convert.ToBase64String(imageBytes);
    }
}
