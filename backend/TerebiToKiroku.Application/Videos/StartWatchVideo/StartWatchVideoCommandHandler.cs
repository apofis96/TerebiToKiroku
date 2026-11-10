using Mediator;
using TerebiToKiroku.Application.Interfaces;
using TerebiToKiroku.Domain.Entities;
using TerebiToKiroku.Domain.Rules;

namespace TerebiToKiroku.Application.Videos.StartWatchVideo
{
    public class StartWatchVideoCommandHandler(IVideoRepository videoRepository, IVideoUniquenessChecker videoUniquenessChecker) : ICommandHandler<StartWatchVideoCommand, Video>
    {
        private readonly IVideoRepository _videoRepository = videoRepository;
        private readonly IVideoUniquenessChecker _videoUniquenessChecker = videoUniquenessChecker;

        public async ValueTask<Video> Handle(StartWatchVideoCommand command, CancellationToken cancellationToken)
        {
            var isUnique = await _videoUniquenessChecker.IsUniqueAsync(
            command.Key,
            cancellationToken);

            var item = Video.CreateNew(command.Key, command.Name, command.Duration, new VideoKeyMustBeUniqueRule(isUnique));

            await _videoRepository.Add(item);

            return item;
        }
    }
}
