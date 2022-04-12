using MarvelousConfigs.BLL.Models;

namespace MarvelousConfigs.BLL.Services
{
    public interface IMicroservicesService
    {
        Task<List<MicroserviceModel>> GetAllMicroservices();
        Task<MicroserviceModel> GetMicroserviceById(int id);
        Task<MicroserviceWithConfigsModel> GetMicroserviceWithConfigsById(int id);
        Task UpdateMicroservice(int id, MicroserviceModel microservice);
    }
}