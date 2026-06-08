using System.Threading.Tasks;
using Autofac;
using MediatR;
using TerebiToKiroku.Application;
using TerebiToKiroku.Application.Configuration.Queries;

namespace TerebiToKiroku.Infrastructure.Processing
{
    public static class QueriesExecutor
    {
        public static async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            using (var scope = CompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();

                return await mediator.Send(query);
            }
        }
    }
}