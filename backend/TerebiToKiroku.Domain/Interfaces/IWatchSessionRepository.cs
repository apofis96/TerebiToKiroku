using TerebiToKiroku.Domain.Entities;

namespace TerebiToKiroku.Domain.Interfaces
{
    public interface IWatchSessionRepository
    {
        Task Add(WatchSession watchSession);
        Task<WatchSession> GetById(Guid id);
        Task<List<WatchSession>> GetByVideoId(string videoId);
        Task<List<WatchSession>> GetByIds(List<Guid> ids);
    }
}
