using Dapper;
using MarvelousConfigs.DAL.Configuration;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;

namespace MarvelousConfigs.DAL.Repositories
{
    public class MicroservicesRepository : BaseRepository, IMicroserviceRepository
    {
        private readonly ILogger<MicroservicesRepository> _logger;

        public MicroservicesRepository(IOptions<DbConfiguration> options, ILogger<MicroservicesRepository> logger) : base(options)
        {
            _logger = logger;
        }

        public async Task<Microservice> GetMicroserviceById(int id)
        {
            _logger.LogInformation($"Request to get microservice by id{id} to DB");

            using IDbConnection connection = ProvideConnection();

            return await connection.QueryFirstOrDefaultAsync<Microservice>
                (Queries.GetMicroserviceById, new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Microservice>> GetAllMicroservices()
        {
            _logger.LogInformation($"Request to get all microservices to DB");

            using IDbConnection connection = ProvideConnection();

            return (await connection.QueryAsync<Microservice>
                (Queries.GetAllMicroservices, commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task UpdateMicroserviceById(int id, Microservice microservice)
        {
            _logger.LogInformation($"Request to update microservice by id{id} to DB");

            using IDbConnection connection = ProvideConnection();

            await connection.QueryAsync
                (Queries.UpdateMicroserviceById, new { Id = id, Url = microservice.Url, Address = microservice.Address },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<MicroserviceWithConfigs> GetMicroserviceWithConfigsById(int id)
        {
            _logger.LogInformation($"Request to get microservice with configs by id{id} to DB");

            using IDbConnection connection = ProvideConnection();

            Dictionary<int, MicroserviceWithConfigs> dict = new Dictionary<int, MicroserviceWithConfigs>();
            int serviceId = 0;

            await connection.QueryAsync<MicroserviceWithConfigs, Config, MicroserviceWithConfigs>
                (Queries.GetMicroserviceWithConfigsById, (service, conf) =>
                {
                    if (serviceId != service.Id)
                    {
                        dict.Add(service.Id, service);
                        serviceId = service.Id;
                        dict[serviceId].Configs = new List<Config>();
                    }

                    dict[serviceId].Configs.Add(conf);
                    return dict[serviceId];
                },
                new { Id = id }, splitOn: "Id", commandType: CommandType.StoredProcedure);

            return dict[serviceId];
        }
    }
}