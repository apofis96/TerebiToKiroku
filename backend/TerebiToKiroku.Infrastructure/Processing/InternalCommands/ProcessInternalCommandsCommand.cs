using MediatR;
using TerebiToKiroku.Application;
using TerebiToKiroku.Application.Configuration.Commands;
using TerebiToKiroku.Infrastructure.Processing.Outbox;

namespace TerebiToKiroku.Infrastructure.Processing.InternalCommands
{
    internal class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
    {

    }
}