using Dapper;
using TerebiToKiroku.Domain.Entities;
using TerebiToKiroku.Domain.Interfaces;
using TerebiToKiroku.Infrastructure.Data;

namespace TerebiToKiroku.Infrastructure.Repositories
{
    [DapperAot]
    public class VideoRepository(TerebiToKirokuConnectionFactory factory) : IVideoRepository
    {
        private readonly TerebiToKirokuConnectionFactory _factory = factory;

        public async Task Add(Video video)
        {
            var query = "INSERT INTO Videos (Id, Name, Key, Duration) VALUES (@Id, @Name, @Key, @Duration)";

            using var connection = _factory.CreateConnection();
            
            await connection.ExecuteAsync(query, new { video.Id, video.Name, video.Key, video.Duration });
        }

        public async Task<List<Video>> GetAll()
        {
            const string query = "SELECT Id, Name, Key, Duration, CreatedAt FROM Videos";

            using var connection = _factory.CreateConnection();

            var result = await connection.QueryAsync<Video>(query);

            return [.. result];
        }

        public async Task<Video> GetById(Guid id)
        {
            const string query = "SELECT Id, Name, Key, Duration, CreatedAt FROM Videos WHERE Id = @Id";

            using var connection = _factory.CreateConnection();

            var video = await connection.QuerySingleOrDefaultAsync<Video>(query, new { Id = id });

            return video ?? throw new InvalidOperationException("Video not found");
        }

        public async Task<List<Video>> GetByIds(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return [];

            const string query = "SELECT Id, Name, Key, Duration, CreatedAt FROM Videos WHERE Id = ANY(@Ids)";

            using var connection = _factory.CreateConnection();

            var result = await connection.QueryAsync<Video>(query, new { Ids = ids.ToArray() });

            return [.. result];
        }

        public async Task<Video> GetByKey(string key)
        {
            const string query = "SELECT Id, Name, Key, Duration, CreatedAt FROM Videos WHERE Key = @Key";

            using var connection = _factory.CreateConnection();

            var video = await connection.QuerySingleOrDefaultAsync<Video>(query, new { Key = key });

            return video ?? throw new InvalidOperationException("Video not found");
        }

        public async Task<List<Video>> GetByKeys(List<string> keys)
        {
            if (keys == null || keys.Count == 0)
                return [];

            const string query = "SELECT Id, Name, Key, Duration, CreatedAt FROM Videos WHERE Key = ANY(@Keys)";

            using var connection = _factory.CreateConnection();

            var result = await connection.QueryAsync<Video>(query, new { Keys = keys.ToArray() });

            return [.. result];
        }
    }
}
