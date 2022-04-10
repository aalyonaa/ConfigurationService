using AutoMapper;
using MarvelousConfigs.BLL.Exeptions;
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

        public async Task<int> AddMicroservice(MicroserviceModel microservice)
        {
            _logger.LogInformation("Adding a new microservice");
            int id = await _rep.AddMicroservice(_map.Map<Microservice>(microservice));
            _logger.LogInformation($"Microservice { id } has been added");
            if (id > 0)
            {
                _cache.Set((Marvelous.Contracts.Enums.Microservice)id, microservice);
                _logger.LogInformation($"Microservice { id } caching");
            }
            return id;
        }

        public async Task UpdateMicroservice(int id, MicroserviceModel microservice)
        {
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException("");
            }
            _logger.LogInformation($"Changing microservice { id }");
            await _rep.UpdateMicroserviceById(id, _map.Map<Microservice>(microservice));
            _logger.LogInformation($"Microservice { id } has been updated");
            _cache.Set((Marvelous.Contracts.Enums.Microservice)id, _map.Map<MicroserviceModel>(await _rep.GetMicroserviceById(id)));
            _logger.LogInformation($"Microservice { id } caching");
        }

        public async Task DeleteMicroservice(int id)
        {
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException("");
            }
            _logger.LogInformation($"Delete microservice { id }");
            await _rep.DeleteOrRestoreMicroserviceById(id, true);
            _logger.LogInformation($"Microservice { id } has been deleted");
            _cache.Remove(id);
            _logger.LogInformation($"Microservice { id } delete from cach");
        }

        public async Task RestoreMicroservice(int id)
        {
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                 => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException("");
            }
            _logger.LogInformation($"Restore microservice { id }");
            await _rep.DeleteOrRestoreMicroserviceById(id, false);
            _logger.LogInformation($"Microservice { id } has been restore");
            _cache.Set((Marvelous.Contracts.Enums.Microservice)id, service);
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
                throw new EntityNotFoundException("");
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
                throw new EntityNotFoundException("");
            }

            var serviceWithConfigs = await _rep.GetMicroserviceWithConfigsById(id);
            _logger.LogInformation($"Microservice { id } with configs has been received");
            return _map.Map<MicroserviceWithConfigsModel>(serviceWithConfigs);
        }
    }
}
