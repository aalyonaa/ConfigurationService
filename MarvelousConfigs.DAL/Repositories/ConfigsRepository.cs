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
            _logger.LogInformation($"Request to get configs for service {name} to DB");

            using IDbConnection connection = ProvideConnection();

            return (await connection.QueryAsync<Config>
                (Queries.GetConfigsByServiceName, new { Name = name }, commandType: CommandType.StoredProcedure)).ToList();
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
                    Value = config.Value,
                    Description = config.Description
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
