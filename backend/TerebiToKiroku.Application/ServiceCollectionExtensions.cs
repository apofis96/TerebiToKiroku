using Mediator;
using Microsoft.Extensions.DependencyInjection;
using TerebiToKiroku.Application.Pipeline;
using TerebiToKiroku.Application.Videos.StartWatchVideo;

namespace TerebiToKiroku.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediator(
                (MediatorOptions options) =>
                {
                    options.Assemblies = [typeof(StartWatchVideoCommand)];
                    options.ServiceLifetime = ServiceLifetime.Scoped;
                }
            );
            return services
                .AddSingleton(typeof(IPipelineBehavior<,>), typeof(ErrorLoggingBehaviour<,>))
                .AddSingleton(typeof(IPipelineBehavior<,>), typeof(MessageValidatorBehaviour<,>));
        }
    }
}
