using MarvelousConfigs.DAL.Entities;

namespace MarvelousConfigs.DAL.Repositories
{
    public interface IMicroserviceRepository
    {
        Task<List<Microservice>> GetAllMicroservices();
        Task<Microservice> GetMicroserviceById(int id);
        Task<MicroserviceWithConfigs> GetMicroserviceWithConfigsById(int id);
        Task UpdateMicroserviceById(int id, Microservice microservice);
    }
}