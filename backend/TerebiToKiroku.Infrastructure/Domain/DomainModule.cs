using Autofac;
using TerebiToKiroku.Application.Videos.DomainServices;
using TerebiToKiroku.Domain.Videos;

namespace TerebiToKiroku.Infrastructure.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VideoUniquenessChecker>()
                .As<IVideoUniquenessChecker>()
                .InstancePerLifetimeScope();
        }
    }
}