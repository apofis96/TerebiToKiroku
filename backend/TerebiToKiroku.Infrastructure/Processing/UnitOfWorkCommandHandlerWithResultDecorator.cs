using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TerebiToKiroku.Application;
using TerebiToKiroku.Application.Configuration.Commands;
using TerebiToKiroku.Domain.SeedWork;
using TerebiToKiroku.Infrastructure.Database;

namespace TerebiToKiroku.Infrastructure.Processing
{
    public class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
    {
        private readonly ICommandHandler<T, TResult> _decorated;

        private readonly IUnitOfWork _unitOfWork;

        private readonly VideosContext _videosContext;

        public UnitOfWorkCommandHandlerWithResultDecorator(
            ICommandHandler<T, TResult> decorated, 
            IUnitOfWork unitOfWork,
            VideosContext videosContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _videosContext = videosContext;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            var result = await this._decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase<TResult>)
            {
                var internalCommand = await _videosContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await this._unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
    }
}