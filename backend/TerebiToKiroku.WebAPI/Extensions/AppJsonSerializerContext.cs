using System.Text.Json.Serialization;
using TerebiToKiroku.Application.Videos.StartWatchVideo;

namespace TerebiToKiroku.WebAPI.Extensions
{
    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
    [JsonSerializable(typeof(StartWatchVideoRequest))]
    public partial class AppJsonSerializerContext : JsonSerializerContext { }
}
