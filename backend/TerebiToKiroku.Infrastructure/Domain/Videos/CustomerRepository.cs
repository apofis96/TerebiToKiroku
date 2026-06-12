using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TerebiToKiroku.Domain.Videos;
using TerebiToKiroku.Infrastructure.Database;

namespace TerebiToKiroku.Infrastructure.Domain.Videos
{
    public class VideoRepository : IVideoRepository
    {
        private readonly VideosContext _context;

        public VideoRepository(VideosContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<List<Video>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Video> GetById(VideoId id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Video>> GetByIds(List<VideoId> ids)
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