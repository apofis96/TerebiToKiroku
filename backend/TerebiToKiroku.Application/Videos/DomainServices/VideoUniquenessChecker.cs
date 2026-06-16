using Dapper;
using TerebiToKiroku.Application.Configuration.Data;
using TerebiToKiroku.Domain.Videos;

namespace TerebiToKiroku.Application.Videos.DomainServices
{
    public class VideoUniquenessChecker : IVideoUniquenessChecker
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public VideoUniquenessChecker(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public bool IsUnique(string videoKey)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT TOP 1 1" +
                               "FROM [videos] AS [Video] " +
                               "WHERE [Video].[Key] = @VideoKey";
            var videosNumber = connection.QuerySingleOrDefault<int?>(sql,
                            new
                            {
                                VideoKey = videoKey
                            });

            return !videosNumber.HasValue;       
        }
    }
}