using Dapper;
using TerebiToKiroku.Application.Interfaces;
using TerebiToKiroku.Infrastructure.Data;

namespace TerebiToKiroku.Infrastructure.Rules
{
    [DapperAot]
    public class VideoUniquenessChecker(TerebiToKirokuConnectionFactory factory) : IVideoUniquenessChecker
    {
        private readonly TerebiToKirokuConnectionFactory _factory = factory;
        public async Task<bool> IsUniqueAsync(string key, CancellationToken cancellationToken)
        {
            const string query = "SELECT COUNT(*) FROM Videos WHERE Key = @Key";
            using var connection = _factory.CreateConnection();
            var count = await connection.QuerySingleAsync<int>(query, new { Key = key });

            return count == 0;
        }
    }
}
