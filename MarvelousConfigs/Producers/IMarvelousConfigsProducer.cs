
namespace MarvelousConfigs.API.RMQ.Producers
{
    public interface IMarvelousConfigsProducer
    {
        Task NotifyAdminAboutErrorToEmail(string mess);
        Task NotifyConfigurationUpdated(int id);
    }
}