using TerebiToKiroku.Domain.SeedWork;
using TerebiToKiroku.Domain.Videos.Rules;
using TerebiToKiroku.Domain.Videos.WatchSessions;

namespace TerebiToKiroku.Domain.Videos
{
    public class Video : Entity, IAggregateRoot
    {
        public VideoId Id { get; private set; }

        public string Name { get; private set; }

        public string Key { get; private set; }

        public int Duration { get; private set; }

        public List<WatchSession> WatchSessions { get; private set; }

        private Video() {}

        private Video(string key, string name, int duration)
        {
            Id = new VideoId(Guid.NewGuid());
            Key = key;
            Name = name;
            Duration = duration;
            WatchSessions = new List<WatchSession>();
            CreatedAt = DateTime.UtcNow;
        }

        public static Video CreateNew(string key, string name, int duration, IVideoUniquenessChecker videoUniquenessChecker)
        {
            CheckRule(new VideoKeyMustBeUniqueRule(videoUniquenessChecker, key));

            return new Video(key, name, duration);
        }
    }
}