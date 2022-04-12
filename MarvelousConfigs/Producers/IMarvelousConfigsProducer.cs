
namespace MarvelousConfigs.API.RMQ.Producers
{
    public interface IMarvelousConfigsProducer
    {
        Task NotifyConfigurationUpdated(int id);
    }
}