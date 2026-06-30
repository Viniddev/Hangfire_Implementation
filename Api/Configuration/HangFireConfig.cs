using Hangfire;

namespace Api.Configuration;

public static class HangFireConfig
{
    public static IServiceCollection AddHangFireSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 5;
        });

        return services;
    }
}
