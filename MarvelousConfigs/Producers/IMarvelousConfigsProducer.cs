﻿
namespace MarvelousConfigs.API.RMQ.Producers
{
    public interface IMarvelousConfigsProducer
    {
        Task NotifyConfigurationAddedOrUpdated(int id);
    }
}