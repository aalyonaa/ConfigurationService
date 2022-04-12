using AutoMapper;
using MarvelousConfigs.BLL.AuthRequestClient;
using MarvelousConfigs.BLL.Cache;
using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Helper.Exceptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MarvelousConfigs.BLL.Services
{
    public class ConfigsService : IConfigsService
    {
        private readonly IConfigsRepository _rep;
        private readonly IMapper _map;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ConfigsService> _logger;
        private readonly IAuthRequestClient _auth;
        private readonly IMemoryCacheExtentions _memory;

        public ConfigsService(IConfigsRepository repository,
            IMapper mapper, IMemoryCache cache, IMemoryCacheExtentions memory,
            ILogger<ConfigsService> logger, IAuthRequestClient auth)
        {
            _rep = repository;
            _map = mapper;
            _cache = cache;
            _logger = logger;
            _auth = auth;
            _memory = memory;
        }

        public async Task UpdateConfigById(int id, ConfigModel config)
        {
            Config conf = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetConfigById(id));

            if (conf == null)
            {
                throw new EntityNotFoundException($"Configuration { id } not found");
            }
            _logger.LogInformation($"Changing configuration { id }");
            await _rep.UpdateConfigById(id, _map.Map<Config>(config));
            _logger.LogInformation($"Configuration { id } has been updated");
            _cache.Set(id, _map.Map<ConfigModel>(((_rep.GetConfigById(id).Result))));
            await _memory.RefreshConfigByServiceId(config.ServiceId);
            _logger.LogInformation($"Configuration { id } caching");
        }

        public async Task<ConfigModel> GetConfigById(int id)
        {
            _logger.LogInformation($"Get configuration { id }");
            Config conf = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                 => _rep.GetConfigById(id));

            if (conf == null)
            {
                throw new EntityNotFoundException($"Configuration { id } not found");
            }
            _logger.LogInformation($"Configuration { id } has been received");
            return _map.Map<ConfigModel>(conf);
        }

        public async Task<List<ConfigModel>> GetAllConfigs()
        {
            _logger.LogInformation($"Getting all configurations");
            var cfg = _map.Map<List<ConfigModel>>(await _rep.GetAllConfigs());
            _logger.LogInformation($"Configurations has been received");
            return cfg;
        }

        public async Task<List<ConfigModel>> GetConfigsByServiceId(int id)
        {
            _logger.LogInformation($"Getting configurations by service id{ id }");
            var configs = _map.Map<List<ConfigModel>>(await _rep.GetConfigsByServiceId(id));
            _logger.LogInformation($"Configurations has been received");
            return configs;
        }

        public async Task<List<ConfigModel>> GetConfigsByService(string token, string name)
        {
            if (!await _auth.GetRestResponse(token))
            {
                throw new ForbiddenException($"Token for { name } validation failed");
            }
            _logger.LogInformation($"Getting configurations by service address { name }");
            List<Config> configs = await _cache.GetOrCreateAsync(name, (ICacheEntry _)
               => _rep.GetConfigsByService(name));
            _logger.LogInformation($"Configurations has been received");
            return _map.Map<List<ConfigModel>>(configs);

        }
    }
}
