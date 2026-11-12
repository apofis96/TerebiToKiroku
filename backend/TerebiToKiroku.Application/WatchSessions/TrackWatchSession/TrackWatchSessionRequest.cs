namespace TerebiToKiroku.Application.WatchSessions.TrackWatchSession
{
    public class TrackWatchSessionRequest
    {
        public required string Key { get; set; }
        public required int SessionDuration { get; set; }
        public required int WatchedDuration { get; set; }
    }
}
