using System.Reflection;
using TerebiToKiroku.Application.Videos.StartWatchVideo;

namespace TerebiToKiroku.Infrastructure.Processing
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(StartWatchVideoCommand).Assembly;
    }
}