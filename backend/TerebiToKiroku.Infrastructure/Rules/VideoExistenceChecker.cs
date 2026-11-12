using Dapper;
using TerebiToKiroku.Application.Interfaces;
using TerebiToKiroku.Infrastructure.Data;

namespace TerebiToKiroku.Infrastructure.Rules
{
    [DapperAot]
    public class VideoExistenceChecker(TerebiToKirokuConnectionFactory factory) : IVideoExistenceChecker
    {
        private readonly TerebiToKirokuConnectionFactory _factory = factory;
        public async Task<bool> IsExistAsync(string key, CancellationToken cancellationToken)
        {
            const string query = "SELECT COUNT(*) FROM Videos WHERE Key = @Key";
            using var connection = _factory.CreateConnection();
            var count = await connection.QuerySingleAsync<int>(query, new { Key = key });

            return count > 0;
        }
    }
}
