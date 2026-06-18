namespace TerebiToKiroku.Application.Videos.StartWatchVideo
{
    public class StartWatchVideoRequest
    {
        public required string Name { get; set; }
        public required string Key { get; set; }
        public required int Duration { get; set; }
    }
}
