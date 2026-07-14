using TerebiToKiroku.Domain.Entities;

namespace TerebiToKiroku.Domain.Interfaces
{
    public interface IVideoRepository
    {
        Task Add(Video video);

        Task<Video> GetById(Guid id);
        Task<Video> GetByKey(string key);
        Task<List<Video>> GetByIds(List<Guid> ids);
        Task<List<Video>> GetByKeys(List<string> keys);

        Task<List<Video>> GetAll();
    }
}
