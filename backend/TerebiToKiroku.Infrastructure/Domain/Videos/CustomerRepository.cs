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

        /*public async Task AddAsync(Customer customer)
        {
            await this._context.Customers.AddAsync(customer);
        }

        public async Task<Customer> GetByIdAsync(CustomerId id)
        {
            return await this._context.Customers
                .IncludePaths(
                    CustomerEntityTypeConfiguration.OrdersList,
                    CustomerEntityTypeConfiguration.OrderProducts)
                .SingleAsync(x => x.Id == id);
        }*/
    }
}