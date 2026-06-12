using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TerebiToKiroku.Application;
using TerebiToKiroku.Application.Configuration.Commands;
using TerebiToKiroku.Domain.SeedWork;
using TerebiToKiroku.Infrastructure.Database;

namespace TerebiToKiroku.Infrastructure.Processing
{
    public class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T:ICommand
    {
        private readonly ICommandHandler<T> _decorated;

        private readonly IUnitOfWork _unitOfWork;

        private readonly VideosContext _videosContext;

        public UnitOfWorkCommandHandlerDecorator(
            ICommandHandler<T> decorated, 
            IUnitOfWork unitOfWork, 
            VideosContext videosContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _videosContext = videosContext;
        }

        public async Task<Unit> Handle(T command, CancellationToken cancellationToken)
        {
            await this._decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase)
            {
                var internalCommand =
                    await _videosContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id,
                        cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await this._unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}