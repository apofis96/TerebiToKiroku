using Mediator;
using TerebiToKiroku.Application.Interfaces;
using TerebiToKiroku.Domain.Entities;

namespace TerebiToKiroku.Application.Videos.StartWatchVideo
{
    public class StartWatchVideoCommandHandler(IVideoRepository videoRepository) : ICommandHandler<StartWatchVideoCommand, Video>
    {
        private readonly IVideoRepository _videoRepository = videoRepository;

        public async ValueTask<Video> Handle(StartWatchVideoCommand command, CancellationToken cancellationToken)
        {
            var item = Video.CreateNew(command.Key, command.Name, command.Duration);

            await _videoRepository.Add(item);

            return item;
        }
    }
}
