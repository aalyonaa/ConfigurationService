using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MarvelousConfigs.BLL.Cache
{
    public class MemoryCacheExtentions : IMemoryCacheExtentions
    {
        private readonly IMemoryCache _cache;
        private readonly IConfigsRepository _config;
        private readonly IMicroserviceRepository _microservice;
        private readonly ILogger<MemoryCacheExtentions> _logger;

        public MemoryCacheExtentions(IMemoryCache cache, IMicroserviceRepository microservice,
            IConfigsRepository configs, ILogger<MemoryCacheExtentions> logger)
        {
            _cache = cache;
            _microservice = microservice;
            _config = configs;
            _logger = logger;
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
                _logger.LogCritical($"Error loading objects into the cache. {ex}");
            }
        }

        public async Task RefreshConfigByServiceId(int id)
        {
            var service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _microservice.GetMicroserviceById(id));

            if (service == null)
                throw new EntityNotFoundException($"Service with id{ id } was not found");

            var configs = await _config.GetConfigsByService(service.ServiceName);
            List<Config> cfgs = new List<Config>();
            foreach (var c in configs)
            {
                cfgs.Add(c);
            }
            _cache.Set(service.ServiceName, cfgs);
        }
    }
}
