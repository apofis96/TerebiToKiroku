using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.Caching.Memory;
using TerebiToKiroku.WebAPI.Configuration;
using TerebiToKiroku.Application.Configuration.Validation;
using TerebiToKiroku.WebAPI.SeedWork;
using TerebiToKiroku.Application.Configuration;
using TerebiToKiroku.Domain.SeedWork;
using TerebiToKiroku.Infrastructure;
using TerebiToKiroku.Infrastructure.Caching;

namespace TerebiToKiroku.WebAPI
{
    public class Startup(IWebHostEnvironment env)
    {
        private readonly IConfiguration _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .Build();

        private const string OrdersConnectionString = "OrdersConnectionString";

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMemoryCache();

            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });


            services.AddHttpContextAccessor();
            var serviceProvider = services.BuildServiceProvider();

            IExecutionContextAccessor executionContextAccessor = new ExecutionContextAccessor(serviceProvider.GetService<IHttpContextAccessor>());

            var children = this._configuration.GetSection("Caching").GetChildren();
            var cachingConfiguration = children.ToDictionary(child => child.Key, child => TimeSpan.Parse(child.Value));
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return ApplicationStartup.Initialize(
                services,
                this._configuration[OrdersConnectionString],
                new MemoryCacheStore(memoryCache, cachingConfiguration),
                executionContextAccessor);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CorrelationMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseProblemDetails();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}