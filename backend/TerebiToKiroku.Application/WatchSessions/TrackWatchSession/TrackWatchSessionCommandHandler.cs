using Mediator;
using TerebiToKiroku.Application.Interfaces;

namespace TerebiToKiroku.Application.WatchSessions.TrackWatchSession
{
    public class TrackWatchSessionCommandHandler(IWatchSessionRepository watchSessionRepository, IVideoExistenceChecker videoExistenceChecker) : ICommandHandler<TrackWatchSessionCommand>
    {
        private readonly IWatchSessionRepository _watchSessionRepository = watchSessionRepository;
        private readonly IVideoExistenceChecker _videoExistenceChecker = videoExistenceChecker;

        public async ValueTask<Unit> Handle(TrackWatchSessionCommand command, CancellationToken cancellationToken)
        {
            var isExist = await _videoExistenceChecker.IsExistAsync(command.Key, cancellationToken);


            throw new NotImplementedException();
        }
    }
}
