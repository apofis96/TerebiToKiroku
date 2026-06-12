using Autofac;
using Microsoft.EntityFrameworkCore;
using TerebiToKiroku.Application.Configuration.Data;
using TerebiToKiroku.Domain.SeedWork;
using TerebiToKiroku.Domain.Videos;
using TerebiToKiroku.Infrastructure.Domain;
using TerebiToKiroku.Infrastructure.Domain.Videos;

namespace TerebiToKiroku.Infrastructure.Database
{
    public class DataAccessModule : Autofac.Module
    {
        private readonly string _databaseConnectionString;

        public DataAccessModule(string databaseConnectionString)
        {
            this._databaseConnectionString = databaseConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", _databaseConnectionString)
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<VideoRepository>()
                .As<IVideoRepository>()
                .InstancePerLifetimeScope();


            builder
                .Register(c =>
                {
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<VideosContext>();
                    dbContextOptionsBuilder.UseNpgsql(_databaseConnectionString);

                    return new VideosContext(dbContextOptionsBuilder.Options);
                })
                .AsSelf()
                .As<DbContext>()
                .InstancePerLifetimeScope();
        }
    }
}