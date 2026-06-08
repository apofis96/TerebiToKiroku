using System.Threading.Tasks;
using MediatR;
using TerebiToKiroku.Application.Configuration.Commands;

namespace TerebiToKiroku.Application.Configuration.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}