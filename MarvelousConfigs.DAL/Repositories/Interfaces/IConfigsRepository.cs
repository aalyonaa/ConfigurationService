﻿using MarvelousConfigs.DAL.Entities;

namespace MarvelousConfigs.DAL.Repositories
{
    public interface IConfigsRepository
    {
        Task<int> AddConfig(Config config);
        Task DeleteOrRestoreConfigById(int id, bool IsDeleted);
        Task<List<Config>> GetAllConfigs();
        Task<Config> GetConfigById(int id);
        Task<List<Config>> GetConfigsByServiceId(int id);
        Task<List<Config>> GetConfigsByService(string ip);
        Task UpdateConfigById(int id, Config config);
    }
}