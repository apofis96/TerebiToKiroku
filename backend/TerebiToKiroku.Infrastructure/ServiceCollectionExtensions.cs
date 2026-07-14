using Microsoft.Extensions.DependencyInjection;
using TerebiToKiroku.Domain.Interfaces;
using TerebiToKiroku.Infrastructure.Data;
using TerebiToKiroku.Infrastructure.Repositories;

namespace TerebiToKiroku.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<TerebiToKirokuConnectionFactory>();
            services.AddTransient<IVideoRepository, VideoRepository>();

            return services;
        }
    }
}
