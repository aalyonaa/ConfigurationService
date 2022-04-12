using Marvelous.Contracts.Configurations;
using Marvelous.Contracts.EmailMessageModels;
using Marvelous.Contracts.Enums;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using MassTransit;

namespace MarvelousConfigs.API.RMQ.Producers
{
    public class MarvelousConfigsProducer : IMarvelousConfigsProducer
    {
        private readonly IConfigsService _config;
        private readonly ILogger<MarvelousConfigsProducer> _logger;
        private readonly IBus _bus;

        public MarvelousConfigsProducer(IConfigsService configsService, ILogger<MarvelousConfigsProducer> logger, IBus bus)
        {
            _config = configsService;
            _logger = logger;
            _bus = bus;
        }

        public async Task NotifyAdminAboutErrorToEmail(string mess)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            _logger.LogInformation($"Try publish message about error for service {Microservice.MarvelousEmailSender}");
            await _bus.Publish<EmailErrorMessage>(new
            {
                ServiceName = Microservice.MarvelousConfigs.ToString(),
                TextMessage = mess
            },
               source.Token);
            _logger.LogInformation($"Message about error for service {Microservice.MarvelousEmailSender} published");
        }

        public async Task NotifyConfigurationUpdated(int id)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var config = await _config.GetConfigById(id);
            _logger.LogInformation($"Try publish config id{id} for {((Microservice)config.ServiceId)}");

            await CheckMicroserviceAndPublish(config, source);
            _logger.LogInformation($"Config id{config.Id} for {((Microservice)config.ServiceId)} published");
        }

        private async Task CheckMicroserviceAndPublish(ConfigModel config, CancellationTokenSource source)
        {
            switch ((Microservice)config.ServiceId)
            {
                case Microservice.MarvelousAccountChecking:
                    await _bus.Publish<AccountCheckingCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousReporting:
                    await _bus.Publish<ReportingCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousResource:
                    await _bus.Publish<ResourceCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousTransactionStore:
                    await _bus.Publish<TransactionStoreCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousCrm:
                    await _bus.Publish<CrmCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousEmailSender:
                    await _bus.Publish<EmailSendlerCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousRatesApi:
                    await _bus.Publish<RatesApiCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousAuth:
                    await _bus.Publish<AuthCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                source.Token);
                    break;

                case Microservice.MarvelousSmsSender:
                    await _bus.Publish<SmsSendlerCfg>(new
                    {
                        config.Key,
                        config.Value
                    },
                 source.Token);
                    break;

                default:
                    throw new Exception($"Unable to send configurations {config.Id} for {(Microservice)config.ServiceId}");

            }
        }
    }
}
