using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Store.Api.Version;

namespace Store.Api.Tests.Version;

public class VersionEndpointsTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetVersion_ReturnsApiAssemblyVersion()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/version");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<VersionResponse>();
        var expected = typeof(VersionEndpoints).Assembly.GetName().Version!.ToString();
        Assert.Equal(expected, body?.Version);
    }
}
