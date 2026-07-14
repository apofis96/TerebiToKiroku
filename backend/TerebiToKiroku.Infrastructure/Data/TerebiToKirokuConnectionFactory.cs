using Microsoft.Extensions.Configuration;
using Npgsql;

using System.Data;

namespace TerebiToKiroku.Infrastructure.Data
{
    public class TerebiToKirokuConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TerebiToKirokuConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
        
    }
}
