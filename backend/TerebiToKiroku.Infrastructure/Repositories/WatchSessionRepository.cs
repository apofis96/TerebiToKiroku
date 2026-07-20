using Dapper;
using TerebiToKiroku.Domain.Entities;
using TerebiToKiroku.Domain.Interfaces;
using TerebiToKiroku.Infrastructure.Data;

namespace TerebiToKiroku.Infrastructure.Repositories
{
    [DapperAot]
    public class WatchSessionRepository(TerebiToKirokuConnectionFactory factory) : IWatchSessionRepository
    {
        private readonly TerebiToKirokuConnectionFactory _factory = factory;

        public async Task Add(WatchSession watchSession)
        {
            const string query = "INSERT INTO WatchSessions (Id, SessionDuration, WatchedDuration, CreatedAt) VALUES (@Id, @SessionDuration, @WatchedDuration, @CreatedAt)";

            using var connection = _factory.CreateConnection();

            await connection.ExecuteAsync(query, new { watchSession.Id, watchSession.SessionDuration, watchSession.WatchedDuration, watchSession.CreatedAt });
        }

        public async Task<WatchSession> GetById(Guid id)
        {
            const string query = "SELECT Id, SessionDuration, WatchedDuration, CreatedAt FROM WatchSessions WHERE Id = @Id";

            using var connection = _factory.CreateConnection();

            var watchSession = await connection.QuerySingleOrDefaultAsync<WatchSession>(query, new { Id = id });

            return watchSession ?? throw new InvalidOperationException("WatchSession not found");
        }

        public async Task<List<WatchSession>> GetByVideoId(string videoId)
        {
            if (string.IsNullOrWhiteSpace(videoId))
                return new List<WatchSession>();

            if (!Guid.TryParse(videoId, out var parsed))
                return new List<WatchSession>();

            const string query = "SELECT Id, SessionDuration, WatchedDuration, CreatedAt FROM WatchSessions WHERE VideoId = @VideoId";

            using var connection = _factory.CreateConnection();

            var result = await connection.QueryAsync<WatchSession>(query, new { VideoId = parsed });

            return result.AsList();
        }

        public async Task<List<WatchSession>> GetByIds(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<WatchSession>();

            const string query = "SELECT Id, SessionDuration, WatchedDuration, CreatedAt FROM WatchSessions WHERE Id = ANY(@Ids)";

            using var connection = _factory.CreateConnection();

            var result = await connection.QueryAsync<WatchSession>(query, new { Ids = ids.ToArray() });

            return result.AsList();
        }
    }
}
