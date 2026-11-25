namespace Store.Api.Caching;

public static class CachingExtensions
{
    public const string ShortPolicy = "Short";

    public static IServiceCollection AddApiCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(CachingOptions.SectionName).Get<CachingOptions>() ?? new CachingOptions();

        services.AddOutputCache(outputCache =>
        {
            outputCache.AddPolicy(ShortPolicy, policy => policy.Expire(TimeSpan.FromSeconds(options.ShortDurationSeconds)));
        });

        return services;
    }
}

public class CachingOptions
{
    public const string SectionName = "Caching";

    public int ShortDurationSeconds { get; set; } = 30;
}
