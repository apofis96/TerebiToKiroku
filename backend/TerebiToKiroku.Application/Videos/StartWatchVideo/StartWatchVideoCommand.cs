using TerebiToKiroku.Application.Configuration.Commands;

namespace TerebiToKiroku.Application.Videos.StartWatchVideo
{
    public class StartWatchVideoCommand : CommandBase<Guid>
    {
        public Guid VideoId { get; }
        public string Name { get; }
        public string Key { get; }
        public int Duration { get; }

        public StartWatchVideoCommand(Guid videoId, string name, string key, int duration)
        {
            this.VideoId = videoId;
            this.Name = name;
            this.Key = key;
            this.Duration = duration;
        }
    }
}