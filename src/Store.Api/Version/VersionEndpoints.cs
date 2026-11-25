using System.Reflection;

namespace Store.Api.Version;

public static class VersionEndpoints
{
    public static IEndpointRouteBuilder MapVersionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/version", () => new VersionResponse(GetAssemblyVersion(typeof(VersionEndpoints).Assembly)))
            .WithName("GetVersion");

        return app;
    }

    public static string GetAssemblyVersion(Assembly assembly) =>
        assembly.GetName().Version?.ToString() ?? "unknown";
}

public record VersionResponse(string Version);
