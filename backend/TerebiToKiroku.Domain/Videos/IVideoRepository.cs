namespace TerebiToKiroku.Domain.Videos
{
    public interface IVideoRepository
    {
        Task<Video> GetById(VideoId id);
        Task<Video> GetByKey(string key);
        Task<List<Video>> GetByIds(List<VideoId> ids);
        Task<List<Video>> GetByKeys(List<string> keys);

        Task<List<Video>> GetAll();
    }
}