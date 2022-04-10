using MarvelousConfigs.BLL.Models;

namespace MarvelousConfigs.BLL.Services
{
    public interface IMicroservicesService
    {
        Task<int> AddMicroservice(MicroserviceModel microservice);
        Task DeleteMicroservice(int id);
        Task<List<MicroserviceModel>> GetAllMicroservices();
        Task<MicroserviceModel> GetMicroserviceById(int id);
        Task<MicroserviceWithConfigsModel> GetMicroserviceWithConfigsById(int id);
        Task RestoreMicroservice(int id);
        Task UpdateMicroservice(int id, MicroserviceModel microservice);
    }
}