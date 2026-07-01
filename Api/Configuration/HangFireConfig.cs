using Hangfire;
using Hangfire.MemoryStorage;

namespace Api.Configuration;

public static class HangFireConfig
{
    public static IServiceCollection AddHangFireSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMemoryStorage());

        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 5;
        });

        return services;
    }
}
