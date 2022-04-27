using Marvelous.Contracts.Configurations;
using Marvelous.Contracts.EmailMessageModels;
using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.DAL.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarvelousConfigs.BLL.Tests
{
    internal class MarvelousConfigsProducerTests : BaseVerifyTest<MarvelousConfigsProducer>
    {
        private Mock<IBus> _bus;
        private MarvelousConfigsProducer _producer;

        [SetUp]
        public void SetUp()
        {
            _bus = new Mock<IBus>();
            _logger = new Mock<ILogger<MarvelousConfigsProducer>>();
            _producer = new MarvelousConfigsProducer(_logger.Object, _bus.Object);
        }

        [Test]
        public async Task NotifyAdminAboutErrorToEmailTest_ValidRequestReceived_ShouldPublishMessage()
        {
            //given
            string message = "test";
            _bus.Setup(x => x.Publish(It.IsAny<EmailErrorMessage>(), It.IsAny<CancellationToken>()));

            //when
            await _producer.NotifyAdminAboutErrorToEmail(message);

            //then
            _bus.Verify(v => v.Publish(It.IsAny<EmailErrorMessage>(), It.IsAny<CancellationToken>()), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public async Task NotifyConfigurationUpdatedTest()
        {
            //given
            Config cfg = new Config() { Id = 3, Created = DateTime.Now, Key = "Key", Value = "Value", ServiceId = 9 };
            _bus.Setup(x => x.Publish(It.IsAny<AuthCfg>(), It.IsAny<CancellationToken>()));

            //when
            await _producer.NotifyConfigurationUpdated(cfg);

            //then
            _bus.Verify(v => v.Publish(It.IsAny<AuthCfg>(), It.IsAny<CancellationToken>()), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void NotifyConfigurationUpdatedTest_WhenMicroserviceNotFound_ShouldThrowException()
        {
            //given
            Config cfg = new Config() { Id = 3, Created = DateTime.Now, Key = "Key", Value = "Value", ServiceId = 100000 };
            _bus.Setup(x => x.Publish(It.IsAny<AuthCfg>(), It.IsAny<CancellationToken>()));
            _bus.Setup(x => x.Publish(It.IsAny<EmailErrorMessage>(), It.IsAny<CancellationToken>()));

            //when       

            //then
            Assert.ThrowsAsync<Exception>(async () => await _producer.NotifyConfigurationUpdated(cfg));
            _bus.Verify(v => v.Publish(It.IsAny<AuthCfg>(), It.IsAny<CancellationToken>()), Times.Never);
            _bus.Verify(v => v.Publish(It.IsAny<EmailErrorMessage>(), It.IsAny<CancellationToken>()), Times.Once);
            VerifyLogger(LogLevel.Information, 3);
        }
    }
}
