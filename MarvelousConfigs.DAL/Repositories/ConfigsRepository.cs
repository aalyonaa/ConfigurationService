using Dapper;
using MarvelousConfigs.DAL.Configuration;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;

namespace MarvelousConfigs.DAL.Repositories
{
    public class ConfigsRepository : BaseRepository, IConfigsRepository
    {
        private readonly ILogger<ConfigsRepository> _logger;

        public ConfigsRepository(IOptions<DbConfiguration> options, ILogger<ConfigsRepository> logger) : base(options)
        {
            _logger = logger;
        }

        public async Task<Config> GetConfigById(int id)
        {
            _logger.LogInformation($"Request to get config by id{id} to DB");

            using IDbConnection connection = ProvideConnection();

            return await connection.QueryFirstOrDefaultAsync<Config>
                (Queries.GetConfigById, new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Config>> GetAllConfigs()
        {
            _logger.LogInformation($"Request to get all configs to DB");

            using IDbConnection connection = ProvideConnection();

            return (await connection.QueryAsync<Config>
                (Queries.GetAllConfigs, commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<List<Config>> GetConfigsByServiceId(int id)
        {
            _logger.LogInformation($"Request to get configs by service id{id} to DB");

            using IDbConnection connection = ProvideConnection();

            return (await connection.QueryAsync<Config>
                (Queries.GetConfigsByServiceId, new { ServiceId = id }, commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<List<Config>> GetConfigsByService(string name)
        {
            _logger.LogInformation($"Request to get configs by microservice address {name} to DB");

            using IDbConnection connection = ProvideConnection();

            return (await connection.QueryAsync<Config>
                (Queries.GetConfigsByServiceAddress, new { Address = name }, commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<int> AddConfig(Config config)
        {
            _logger.LogInformation("Request to add a new configuration to DB");

            using IDbConnection connection = ProvideConnection();

            return await connection.QuerySingleAsync<int>
                (Queries.AddConfig, new
                {
                    Key = config.Key,
                    Value = config.Value,
                    ServiceId = config.ServiceId,
                    Description = config.Description
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateConfigById(int id, Config config)
        {
            _logger.LogInformation($"Request to update config by id{id} to DB");

            using IDbConnection connection = ProvideConnection();

            await connection.QueryAsync
                (Queries.UpdateConfigById,
                new
                {
                    Id = id,
                    Key = config.Key,
                    Value = config.Value,
                    ServiceId = config.ServiceId,
                    Description = config.Description
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteOrRestoreConfigById(int id, bool isDeleted)
        {
            _logger.LogInformation($"Request to update config by id{id} to DB");

            using IDbConnection connection = ProvideConnection();

            await connection.QueryAsync
                (Queries.DeleteOrRestoreConfigById, new { Id = id, IsDeleted = isDeleted }, commandType: CommandType.StoredProcedure);
        }
    }
}
