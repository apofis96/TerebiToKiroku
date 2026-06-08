using System.Threading.Tasks;

namespace TerebiToKiroku.Infrastructure.Processing
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}