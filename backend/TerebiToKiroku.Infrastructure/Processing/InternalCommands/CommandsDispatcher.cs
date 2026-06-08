using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TerebiToKiroku.Application.Configuration.Processing;
using TerebiToKiroku.Application.Videos;
using TerebiToKiroku.Infrastructure.Database;

namespace TerebiToKiroku.Infrastructure.Processing.InternalCommands
{
    public class CommandsDispatcher : ICommandsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly VideosContext _videosContext;

        public CommandsDispatcher(
            IMediator mediator, 
            VideosContext videosContext)
        {
            this._mediator = mediator;
            this._videosContext = videosContext;
        }

        public async Task DispatchCommandAsync(Guid id)
        {
            var internalCommand = await this._videosContext.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);

            Type type = Assembly.GetAssembly(typeof(MarkCustomerAsWelcomedCommand)).GetType(internalCommand.Type);
            dynamic command = JsonSerializer.Deserialize(internalCommand.Data, type);

            internalCommand.ProcessedDate = DateTime.UtcNow;

            await this._mediator.Send(command);
        }
    }
}