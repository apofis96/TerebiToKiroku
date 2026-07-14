using Dapper;
using System.Data;
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

        public Task<List<Video>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Video> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Video>> GetByIds(List<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<Video> GetByKey(string key)
        {
            throw new NotImplementedException();
        }

        public Task<List<Video>> GetByKeys(List<string> keys)
        {
            throw new NotImplementedException();
        }
    }
}
