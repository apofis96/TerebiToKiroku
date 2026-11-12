using System.Text.Json.Serialization;
using TerebiToKiroku.Application.Videos.StartWatchVideo;
using TerebiToKiroku.Application.WatchSessions.TrackWatchSession;

namespace TerebiToKiroku.WebAPI.Extensions
{
    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
    [JsonSerializable(typeof(StartWatchVideoRequest))]
    [JsonSerializable(typeof(TrackWatchSessionRequest))]
    public partial class AppJsonSerializerContext : JsonSerializerContext { }
}
