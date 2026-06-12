using TerebiToKiroku.Domain.SeedWork;

namespace TerebiToKiroku.Domain.Videos.WatchSessions
{
    public class WatchSession : Entity
    {
        public WatchSessionId Id { get; private set; }
        public int SessionDuration { get; private set; }
        public int WatchedDuration { get; private set; }

        private WatchSession() { }

        public WatchSession(int sessionDuration, int watchedDuration)
        {
            Id = new WatchSessionId(Guid.NewGuid());
            SessionDuration = sessionDuration;
            WatchedDuration = watchedDuration;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
