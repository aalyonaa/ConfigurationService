
using MarvelousConfigs.DAL.Entities;

namespace MarvelousConfigs.BLL.Infrastructure
{
    public interface IMarvelousConfigsProducer
    {
        Task NotifyAdminAboutErrorToEmail(string mess);
        Task NotifyConfigurationUpdated(Config config);
    }
}