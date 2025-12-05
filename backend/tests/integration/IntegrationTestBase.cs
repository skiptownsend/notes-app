using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace NotesApi.IntegrationTests;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;
    protected readonly WebApplicationFactory<Program> Factory;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        Factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Test");

            // Override services for testing if needed
            builder.ConfigureServices(services =>
            {
                // Add or replace services here for testing
                // Example: Replace database context with in-memory database
            });
        });

        Client = Factory.CreateClient();
    }
}
