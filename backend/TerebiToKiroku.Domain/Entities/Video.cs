namespace TerebiToKiroku.Domain.Entities
{
    public class Video : Entity
    {
        public string Name { get; set; }

        public string Key { get; set; }

        public int Duration { get; set; }

        public List<WatchSession> WatchSessions { get; set; }

        public Video() { }

        private Video(string key, string name, int duration)
        {
            Id = Guid.NewGuid();
            Key = key;
            Name = name;
            Duration = duration;
            WatchSessions = new List<WatchSession>();
            CreatedAt = DateTime.UtcNow;
        }

        public static Video CreateNew(string key, string name, int duration)
        {
            //CheckRule(new VideoKeyMustBeUniqueRule(videoUniquenessChecker, key));

            return new Video(key, name, duration);
        }
    }
}
