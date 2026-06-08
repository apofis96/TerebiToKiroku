using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;
using TerebiToKiroku.Application.Configuration;
using TerebiToKiroku.Infrastructure.Caching;
using TerebiToKiroku.Infrastructure.Database;
using TerebiToKiroku.Infrastructure.Domain;
using TerebiToKiroku.Infrastructure.Processing;

namespace TerebiToKiroku.Infrastructure
{
    public class ApplicationStartup
    {
        public static IServiceProvider Initialize(
            IServiceCollection services,
            string connectionString,
            ICacheStore cacheStore,
            IExecutionContextAccessor executionContextAccessor)
        {

            services.AddSingleton(cacheStore);

            var serviceProvider = CreateAutofacServiceProvider(
                services,
                connectionString,
                executionContextAccessor);

            return serviceProvider;
        }

        private static IServiceProvider CreateAutofacServiceProvider(
            IServiceCollection services,
            string connectionString,
            IExecutionContextAccessor executionContextAccessor)
        {
            var container = new ContainerBuilder();

            container.Populate(services);

            container.RegisterModule(new DataAccessModule(connectionString));
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new DomainModule());

            container.RegisterModule(new ProcessingModule());

            container.RegisterInstance(executionContextAccessor);

            var buildContainer = container.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(buildContainer));

            var serviceProvider = new AutofacServiceProvider(buildContainer);

            CompositionRoot.SetContainer(buildContainer);

            return serviceProvider;
        }
    }
}