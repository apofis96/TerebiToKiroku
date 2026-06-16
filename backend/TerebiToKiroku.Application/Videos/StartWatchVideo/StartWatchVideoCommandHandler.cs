using TerebiToKiroku.Application.Configuration.Commands;
using TerebiToKiroku.Domain.SeedWork;
using TerebiToKiroku.Domain.Videos;

namespace TerebiToKiroku.Application.Videos.StartWatchVideo
{
    public class StartWatchVideoCommandHandler : ICommandHandler<StartWatchVideoCommand, Guid>
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IVideoUniquenessChecker _videoUniquenessChecker;
        private readonly IUnitOfWork _unitOfWork;

        public StartWatchVideoCommandHandler(
            IVideoRepository videoRepository,
            IVideoUniquenessChecker videoUniquenessChecker,
            IUnitOfWork unitOfWork)
        {
            this._videoRepository = videoRepository;
            this._videoUniquenessChecker = videoUniquenessChecker;
            this._unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(StartWatchVideoCommand command, CancellationToken cancellationToken)
        {
            var video = Video.CreateNew(command.Key, command.Name, command.Duration, this._videoUniquenessChecker);

            await this._videoRepository.Add(video);

            await this._unitOfWork.CommitAsync(cancellationToken);

            return video.Id.Value;
        }
    }
}