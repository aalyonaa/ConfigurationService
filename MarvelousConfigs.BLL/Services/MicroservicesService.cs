using AutoMapper;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microservice = MarvelousConfigs.DAL.Entities.Microservice;

namespace MarvelousConfigs.BLL.Services
{
    public class MicroservicesService : IMicroservicesService
    {
        private readonly IMicroserviceRepository _rep;
        private readonly IMemoryCache _cache;
        private readonly IMapper _map;
        private readonly ILogger<MicroservicesService> _logger;

        public MicroservicesService(IMicroserviceRepository repository, IMapper mapper, IMemoryCache cache, ILogger<MicroservicesService> logger)
        {
            _rep = repository;
            _map = mapper;
            _cache = cache;
            _logger = logger;
        }

        public async Task UpdateMicroservice(int id, MicroserviceModel microservice)
        {
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException($"Service with id{ id } was not found");
            }

            _logger.LogInformation($"Start update microservice { id }");
            await _rep.UpdateMicroserviceById(id, _map.Map<Microservice>(microservice));
            _logger.LogInformation($"Microservice { id } has been updated");
            _logger.LogInformation($"Start changing updated microservice { id }");
            _cache.Set((Marvelous.Contracts.Enums.Microservice)id, _map.Map<MicroserviceModel>(await _rep.GetMicroserviceById(id)));
            _logger.LogInformation($"Microservice { id } caching");
        }

        public async Task<List<MicroserviceModel>> GetAllMicroservices()
        {
            _logger.LogInformation($"Getting all microservices");
            var services = _map.Map<List<MicroserviceModel>>(await _rep.GetAllMicroservices());
            _logger.LogInformation($"Microservices has been received");
            return services;
        }

        public async Task<MicroserviceModel> GetMicroserviceById(int id)
        {
            _logger.LogInformation($"Get microservice { id }");
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException($"Service with id{ id } was not found");
            }

            _logger.LogInformation($"Microservice { id } has been received");
            return _map.Map<MicroserviceModel>(service);
        }

        public async Task<MicroserviceWithConfigsModel> GetMicroserviceWithConfigsById(int id)
        {
            _logger.LogInformation($"Get microservice { id } with configs");
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException($"Service with id{ id } was not found");
            }

            var serviceWithConfigs = await _rep.GetMicroserviceWithConfigsById(id);
            _logger.LogInformation($"Microservice { id } with configs has been received");
            return _map.Map<MicroserviceWithConfigsModel>(serviceWithConfigs);
        }
    }
}
