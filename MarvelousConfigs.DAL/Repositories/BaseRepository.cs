using MarvelousConfigs.DAL.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace MarvelousConfigs.DAL.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        public string ConnectionString { get; set; }

        public BaseRepository(IOptions<DbConfiguration> options)
        {
            ConnectionString = options.Value.ConnectionString;
        }

        protected IDbConnection ProvideConnection() => new SqlConnection(ConnectionString);
    }
}
