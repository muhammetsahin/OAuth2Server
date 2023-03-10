using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator
{
    public static class ServiceProvider
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, Assembly[] assemblies)
        {
            return services
                .AddMediatR(config => { config.Using<Mediator>().AsSingleton(); }, assemblies)
                .AddSingleton<IMediator, Mediator>();
        }
    }
}