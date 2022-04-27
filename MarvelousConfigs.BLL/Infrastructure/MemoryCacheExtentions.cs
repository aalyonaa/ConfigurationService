using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MarvelousConfigs.BLL.Infrastructure
{
    public class MemoryCacheExtentions : IMemoryCacheExtentions
    {
        private readonly IMemoryCache _cache;
        private readonly IMarvelousConfigsProducer _prod;
        private readonly IConfigsRepository _config;
        private readonly IMicroserviceRepository _microservice;
        private readonly ILogger<MemoryCacheExtentions> _logger;

        public MemoryCacheExtentions(IMemoryCache cache, IMicroserviceRepository microservice,
            IConfigsRepository configs, ILogger<MemoryCacheExtentions> logger, IMarvelousConfigsProducer producer)
        {
            _cache = cache;
            _microservice = microservice;
            _config = configs;
            _logger = logger;
            _prod = producer;
        }

        public async Task SetMemoryCache()
        {
            try
            {
                _logger.LogInformation("Start loading objects into the cache");
                var services = await _microservice.GetAllMicroservices();
                foreach (var c in services)
                {
                    _cache.Set((Marvelous.Contracts.Enums.Microservice)c.Id, c);
                }

                var configs = await _config.GetAllConfigs();
                foreach (var config in configs)
                {
                    _cache.Set(config.Id, config);
                }

                foreach (var s in services)
                {
                    List<Config> cfgs = new List<Config>();
                    foreach (var c in configs)
                    {
                        if (s.Id == c.ServiceId)
                        {
                            cfgs.Add(c);
                        }
                    }
                    _cache.Set(s.ServiceName, cfgs);
                }
                _logger.LogInformation("Objects were successfully loaded into the cache");
            }
            catch (Exception ex)
            {
                await _prod.NotifyAdminAboutErrorToEmail($"Cache loading error during service initialization. {ex}");
                throw new CacheLoadingException($"Cache loading error during service initialization. {ex}");
            }
        }

        public async Task RefreshConfigByServiceId(int id)
        {
            try
            {
                _logger.LogInformation($"Start try update configurations into the cache for service {id}");
                var service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                    => _microservice.GetMicroserviceById(id));

                if (service == null)
                    throw new EntityNotFoundException($"Service with id{ id } was not found");

                var configs = await _config.GetConfigsByService(service.ServiceName);
                _logger.LogInformation($"Update configurations into the cache for {(Marvelous.Contracts.Enums.Microservice)id}");
                List<Config> cfgs = new List<Config>();
                foreach (var c in configs)
                {
                    cfgs.Add(c);
                }
                _cache.Set(service.ServiceName, cfgs);
                _logger.LogInformation("New configurations were successfully loaded into the cache");
            }
            catch (Exception exception)
            {
                var ex = new CacheLoadingException($"Сache loading error when trying to update cached configurations for service " +
                    $"{(Marvelous.Contracts.Enums.Microservice)id}. {exception.Message}");
                await _prod.NotifyAdminAboutErrorToEmail(ex.Message);
                throw ex;
            }
        }
    }
}
