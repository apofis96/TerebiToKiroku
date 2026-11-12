using TerebiToKiroku.Domain.Abstractions;
using TerebiToKiroku.Domain.Rules;

namespace TerebiToKiroku.Domain.Entities
{
    public class WatchSession : Entity
    {
        public required string Key { get; set; }
        public int SessionDuration { get; private set; }
        public int WatchedDuration { get; private set; }

        private WatchSession() { }

        private WatchSession(int sessionDuration, int watchedDuration)
        {
            Id = Guid.NewGuid();
            SessionDuration = sessionDuration;
            WatchedDuration = watchedDuration;
            CreatedAt = DateTime.UtcNow;
        }

        public static WatchSession CreateNew(string key, int sessionDuration, int watchedDuration, VideoKeyMustExistRule videoExistenceRule)
        {
            CheckRule(videoExistenceRule);

            return new WatchSession(sessionDuration, watchedDuration);
        }
    }
}
