using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Store.Api.Tests.Caching;

public class CachingTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetWeatherForecast_SecondRequest_IsServedFromCache()
    {
        var client = factory.CreateClient();

        var first = await client.GetAsync("/weatherforecast");
        var second = await client.GetAsync("/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);
        Assert.Equal(await first.Content.ReadAsStringAsync(), await second.Content.ReadAsStringAsync());
        Assert.NotNull(second.Headers.Age);
    }

    [Fact]
    public async Task GetWeatherForecast_ExpiredCache_ReturnsFreshResponse()
    {
        var client = factory
            .WithWebHostBuilder(builder => builder.UseSetting("Caching:ShortDurationSeconds", "1"))
            .CreateClient();

        var first = await client.GetAsync("/weatherforecast");
        await Task.Delay(TimeSpan.FromSeconds(1.5));
        var second = await client.GetAsync("/weatherforecast");

        Assert.Null(first.Headers.Age);
        Assert.Null(second.Headers.Age);
    }
}
