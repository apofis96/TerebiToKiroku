namespace TerebiToKiroku.Domain.Entities
{
    public class WatchSession : Entity
    {
        public int SessionDuration { get; private set; }
        public int WatchedDuration { get; private set; }

        private WatchSession() { }

        public WatchSession(int sessionDuration, int watchedDuration)
        {
            Id = Guid.NewGuid();
            SessionDuration = sessionDuration;
            WatchedDuration = watchedDuration;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
