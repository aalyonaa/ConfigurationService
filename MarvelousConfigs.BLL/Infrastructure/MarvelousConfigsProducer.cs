using Marvelous.Contracts.Configurations;
using Marvelous.Contracts.EmailMessageModels;
using MarvelousConfigs.DAL.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microservice = Marvelous.Contracts.Enums.Microservice;

namespace MarvelousConfigs.BLL.Infrastructure
{
    public class MarvelousConfigsProducer : IMarvelousConfigsProducer
    {
        private readonly ILogger<MarvelousConfigsProducer> _logger;
        private readonly IBus _bus;

        public MarvelousConfigsProducer(ILogger<MarvelousConfigsProducer> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task NotifyAdminAboutErrorToEmail(string mess)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            _logger.LogInformation($"Try publish message about error for service {Microservice.MarvelousEmailSender}");
            await _bus.Publish(new EmailErrorMessage
            {
                ServiceName = Microservice.MarvelousConfigs.ToString(),
                TextMessage = mess
            },
               source.Token);
            _logger.LogInformation($"Message about error for service {Microservice.MarvelousEmailSender} published");
        }

        public async Task NotifyConfigurationUpdated(Config config)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            _logger.LogInformation($"Try publish updated config id{config.Id} for {((Marvelous.Contracts.Enums.Microservice)config.ServiceId)}");

            await CheckMicroserviceAndPublish(config, source);
            _logger.LogInformation($"Config id{config.Id} for {(Microservice)config.ServiceId} published");
        }

        private async Task CheckMicroserviceAndPublish(Config config, CancellationTokenSource source)
        {
            switch ((Microservice)config.ServiceId)
            {
                case Marvelous.Contracts.Enums.Microservice.MarvelousAccountChecking:
                    await _bus.Publish(new AccountCheckingCfg
                    {
                        Key = config.Key,
                        Value = config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousResource:
                    await _bus.Publish(new ResourceCfg
                    {
                        Key = config.Key,
                        Value = config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousCrm:
                    await _bus.Publish(new CrmCfg
                    {
                        Key = config.Key,
                        Value = config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousEmailSender:
                    await _bus.Publish(new EmailSendlerCfg
                    {
                        Key = config.Key,
                        Value = config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousRatesApi:
                    await _bus.Publish(new RatesApiCfg
                    {
                        Key = config.Key,
                        Value = config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousAuth:
                    await _bus.Publish(new AuthCfg
                    {
                        Key = config.Key,
                        Value = config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousSmsSender:
                    await _bus.Publish(new SmsSendlerCfg
                    {
                        Key = config.Key,
                        Value = config.Value
                    },
                 source.Token);
                    break;

                default:
                    await NotifyAdminAboutErrorToEmail($"Error for sending updated configuration {config.Id} over RMQ. " +
                        $"Send request for a service {config.ServiceId}, there is no information about this recipient");
                    throw new Exception($"Unable to send configurations {config.Id}. Unknown recipient");

            }
        }
    }
}
