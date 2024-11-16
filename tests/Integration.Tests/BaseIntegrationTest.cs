namespace Integration.Tests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient? _httpClient;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _httpClient = factory.Client;
    }
}
