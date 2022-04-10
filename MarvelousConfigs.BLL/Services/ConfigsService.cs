using AutoMapper;
using MarvelousConfigs.BLL.AuthRequestClient;
using MarvelousConfigs.BLL.Exeptions;
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

        public ConfigsService(IConfigsRepository repository, IMapper mapper, IMemoryCache cache, ILogger<ConfigsService> logger, IAuthRequestClient auth)
        {
            _rep = repository;
            _map = mapper;
            _cache = cache;
            _logger = logger;
            _auth = auth;
        }

        public async Task<int> AddConfig(ConfigModel config)
        {
            _logger.LogInformation("Adding a new configuration");
            int id = await _rep.AddConfig(_map.Map<Config>(config));
            _logger.LogInformation($"Configuration { id } has been added");

            if (id > 0)
            {
                _cache.Set(id, config);
                _logger.LogInformation($"Configuration { id } caching");
            }
            return id;
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
            _logger.LogInformation($"Configuration { id } caching");
        }

        public async Task DeleteConfigById(int id)
        {

            Config conf = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetConfigById(id));

            if (conf == null)
            {
                throw new EntityNotFoundException($"Configuration { id } not found");
            }
            _logger.LogInformation($"Delete configuration { id }");
            await _rep.DeleteOrRestoreConfigById(id, true);
            _logger.LogInformation($"Configuration { id } has been deleted");
            _cache.Remove(id);
            _logger.LogInformation($"Configuration { id } delete from cach");
        }

        public async Task RestoreConfigById(int id)
        {
            Config conf = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetConfigById(id));

            if (conf == null)
            {
                throw new EntityNotFoundException($"Configuration { id } not found");
            }
            _logger.LogInformation($"Restore configuration { id }");
            await _rep.DeleteOrRestoreConfigById(id, false);
            _logger.LogInformation($"Configuration { id } has been restored");
            _cache.Set(id, conf);
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

        public async Task<List<ConfigModel>> GetConfigsByService(string token, string ip)
        {
            if (!await _auth.GetRestResponse(token))
            {
                throw new Exception($"Token for {ip} validation failed");
            }
            _logger.LogInformation($"Getting configurations by service address { ip }");
            List<Config> configs = await _cache.GetOrCreateAsync(ip, (ICacheEntry _)
               => _rep.GetConfigsByService(ip));
            _logger.LogInformation($"Configurations has been received");
            return _map.Map<List<ConfigModel>>(configs);

        }
    }
}
