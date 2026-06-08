using System.Threading;
using System.Threading.Tasks;
using TerebiToKiroku.Infrastructure.Database;
using TerebiToKiroku.Infrastructure.Processing;

namespace TerebiToKiroku.Infrastructure.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VideosContext _videosContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public UnitOfWork(
            VideosContext videosContext,
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            this._videosContext = videosContext;
            this._domainEventsDispatcher = domainEventsDispatcher;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            await this._domainEventsDispatcher.DispatchEventsAsync();
            return await this._videosContext.SaveChangesAsync(cancellationToken);
        }
    }
}