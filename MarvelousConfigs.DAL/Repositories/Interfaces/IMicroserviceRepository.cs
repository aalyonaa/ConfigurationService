using MarvelousConfigs.DAL.Entities;

namespace MarvelousConfigs.DAL.Repositories
{
    public interface IMicroserviceRepository
    {
        Task<int> AddMicroservice(Microservice microservice);
        Task DeleteOrRestoreMicroserviceById(int id, bool isDeleted);
        Task<List<Microservice>> GetAllMicroservices();
        Task<Microservice> GetMicroserviceById(int id);
        Task<MicroserviceWithConfigs> GetMicroserviceWithConfigsById(int id);
        Task UpdateMicroserviceById(int id, Microservice microservice);
    }
}