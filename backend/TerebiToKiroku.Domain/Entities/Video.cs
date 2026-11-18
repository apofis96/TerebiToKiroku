using TerebiToKiroku.Domain.Abstractions;
using TerebiToKiroku.Domain.Rules;

namespace TerebiToKiroku.Domain.Entities
{
    public class Video : Entity
    {
        public string Name { get; set; }

        public required string Key { get; set; }

        public int Duration { get; set; }

        public List<WatchSession> WatchSessions { get; set; }

        public Video() { }

        private Video(string name, int duration)
        {
            Id = Guid.NewGuid();
            Name = name;
            Duration = duration;
            WatchSessions = [];
            CreatedAt = DateTime.UtcNow;
        }

        public static Video CreateNew(string key, string name, int duration, VideoKeyMustBeUniqueRule videoUniquenessRule)
        {
            CheckRule(videoUniquenessRule);

            return new Video(name, duration) { Key = key };
        }
    }
}
