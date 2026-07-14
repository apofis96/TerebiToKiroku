namespace TerebiToKiroku.Domain.Entities
{
    public class Video : Entity
    {
        public string Name { get; private set; }

        public string Key { get; private set; }

        public int Duration { get; private set; }

        public List<WatchSession> WatchSessions { get; private set; }

        private Video() { }

        private Video(string key, string name, int duration)
        {
            Id = Guid.NewGuid();
            Key = key;
            Name = name;
            Duration = duration;
            WatchSessions = [];
            CreatedAt = DateTime.UtcNow;
        }

        public static Video CreateNew(string key, string name, int duration)
        {
            //CheckRule(new VideoKeyMustBeUniqueRule(videoUniquenessChecker, key));

            return new Video(key, name, duration);
        }
    }
}
