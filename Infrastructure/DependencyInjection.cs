using Domain.Abstraction;
using HangFire.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddScoped<IScheduler, Scheduler>();
        service.AddSingleton<IIntegracaoExemplo, IntegracaoExemplo>();

        return service;
    }
}
