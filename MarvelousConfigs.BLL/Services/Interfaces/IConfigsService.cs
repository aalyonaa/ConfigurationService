using MarvelousConfigs.BLL.Models;

namespace MarvelousConfigs.BLL.Services
{
    public interface IConfigsService
    {
        Task<List<ConfigModel>> GetAllConfigs();
        Task<ConfigModel> GetConfigById(int id);
        Task<List<ConfigModel>> GetConfigsByServiceId(int id);
        Task<List<ConfigModel>> GetConfigsByService(string token, string name);
        Task UpdateConfigById(int id, ConfigModel config);
    }
}