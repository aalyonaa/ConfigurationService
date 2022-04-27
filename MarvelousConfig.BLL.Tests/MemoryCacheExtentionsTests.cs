using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarvelousConfigs.BLL.Tests
{
    internal class MemoryCacheExtentionsTests : BaseVerifyTest<MemoryCacheExtentions>
    {
        private Mock<IMarvelousConfigsProducer> _prod;
        private Mock<IConfigsRepository> _config;
        private Mock<IMicroserviceRepository> _microservice;
        private IMemoryCacheExtentions _extentions;

        [SetUp]
        public void Setup()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _microservice = new Mock<IMicroserviceRepository>();
            _config = new Mock<IConfigsRepository>();
            _logger = new Mock<ILogger<MemoryCacheExtentions>>();
            _prod = new Mock<IMarvelousConfigsProducer>();
            _extentions = new MemoryCacheExtentions(_cache, _microservice.Object, _config.Object, _logger.Object, _prod.Object);
        }

        [TestCaseSource(typeof(SetMemoryCacheTestCaseSource))]
        public async Task SetMemoryCacheTest(List<Config> configs, List<Microservice> services)
        {
            //given
            _microservice.Setup(x => x.GetAllMicroservices()).ReturnsAsync(services);
            _config.Setup(x => x.GetAllConfigs()).ReturnsAsync(configs);

            //when
            await _extentions.SetMemoryCache();

            //then
            _microservice.Verify(x => x.GetAllMicroservices(), Times.Once);
            _config.Verify(x => x.GetAllConfigs(), Times.Once);
            VerifyLogger(LogLevel.Information, 2);

            foreach (var s in services)
            {
                List<Config> actualCfg = (List<Config>)_cache.Get(s.ServiceName);
                foreach (var a in actualCfg)
                {
                    Assert.AreEqual(a.ServiceId, s.Id);
                }
            }
        }

        [TestCaseSource(typeof(SetMemoryCacheNegativeTestCaseSource))]
        public void SetMemoryCacheTest_WhenExceptionOccurredWhileCacheLoading_ShouldThrowCacheLoadingException(List<Config> configs, List<Microservice> services)
        {
            //given
            _microservice.Setup(x => x.GetAllMicroservices()).ReturnsAsync(services);
            _config.Setup(x => x.GetAllConfigs()).ReturnsAsync(configs);
            _prod.Setup(x => x.NotifyAdminAboutErrorToEmail(It.IsAny<string>()));

            //when

            //then
            Assert.ThrowsAsync<CacheLoadingException>(async () => await _extentions.SetMemoryCache());
            _microservice.Verify(x => x.GetAllMicroservices(), Times.Once);
            _config.Verify(x => x.GetAllConfigs(), Times.Once);
            _prod.Verify(x => x.NotifyAdminAboutErrorToEmail(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(RefreshConfigByServiceIdTestCaseSource))]
        public async Task RefreshConfigByServiceIdTest(int id, List<Config> configs, Microservice service)
        {
            //given
            _microservice.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(service);
            _config.Setup(x => x.GetConfigsByService(service.ServiceName)).ReturnsAsync(configs);

            //when
            await _extentions.RefreshConfigByServiceId(id);

            //then
            _microservice.Verify(x => x.GetMicroserviceById(id), Times.Once);
            _config.Verify(x => x.GetConfigsByService(service.ServiceName), Times.Once);
            VerifyLogger(LogLevel.Information, 3);

            List<Config> actualCfg = (List<Config>)_cache.Get(service.ServiceName);
            foreach (var a in actualCfg)
            {
                Assert.AreEqual(a.ServiceId, service.Id);
            }
        }

        [TestCaseSource(typeof(RefreshConfigByServiceIdNegativeTestCaseSource))]
        public void RefreshConfigByServiceIdTest_WhenExceptionOccurredWhileCacheLoading_ShouldThrowCacheLoadingException(int id, List<Config> configs, Microservice service)
        {
            //given
            _microservice.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(service);
            _config.Setup(x => x.GetConfigsByService(service.ServiceName)).ReturnsAsync(configs);
            _prod.Setup(x => x.NotifyAdminAboutErrorToEmail(It.IsAny<string>()));

            //when

            //then
            Assert.ThrowsAsync<CacheLoadingException>(async () => await _extentions.RefreshConfigByServiceId(id));
            _microservice.Verify(x => x.GetMicroserviceById(id), Times.Once);
            _config.Verify(x => x.GetConfigsByService(service.ServiceName), Times.Once);
            _prod.Verify(x => x.NotifyAdminAboutErrorToEmail(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void RefreshConfigByServiceIdTest_WhenServiceNotFoundAndThrowEntityNotFoundException_ShouldThrowCacheLoadingException()
        {
            //given
            _microservice.Setup(x => x.GetMicroserviceById(It.IsAny<int>()));

            //when

            //then
            Assert.ThrowsAsync<CacheLoadingException>(async () => await _extentions.RefreshConfigByServiceId(It.IsAny<int>()));
            _microservice.Verify(x => x.GetMicroserviceById(It.IsAny<int>()), Times.Once);
            _config.Verify(x => x.GetConfigsByService(It.IsAny<string>()), Times.Never);
            _prod.Verify(x => x.NotifyAdminAboutErrorToEmail(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 1);
        }
    }
}
