using TerebiToKiroku.Application.Configuration.Commands;

namespace TerebiToKiroku.Application.Videos.StartWatchVideo
{
    public class StartWatchVideoCommand : CommandBase<Guid>
    {
        public string Name { get; }
        public string Key { get; }
        public int Duration { get; }

        public StartWatchVideoCommand(string name, string key, int duration)
        {
            this.Name = name;
            this.Key = key;
            this.Duration = duration;
        }
    }
}