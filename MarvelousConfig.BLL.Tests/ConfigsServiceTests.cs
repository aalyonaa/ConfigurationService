using AutoMapper;
using MarvelousConfigs.BLL.AuthRequestClient;
using MarvelousConfigs.BLL.Configuration;
using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarvelousConfig.BLL.Tests
{
    public class ConfigsServiceTests
    {
        private Mock<IConfigsRepository> _repositoryMock;
        private IMapper _map;
        private IConfigsService _service;
        private IMemoryCache _cache;
        private Mock<ILogger<ConfigsService>> _logger;
        private Mock<IAuthRequestClient> _auth;

        [SetUp]
        public void Setup()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _repositoryMock = new Mock<IConfigsRepository>();
            _logger = new Mock<ILogger<ConfigsService>>();
            _auth = new Mock<IAuthRequestClient>();
            _map = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CustomMapperBLL>()));
            _service = new ConfigsService(_repositoryMock.Object, _map, _cache, _logger.Object, _auth.Object);

        }

        [TestCaseSource(typeof(AddConfigTestCaseSource))]
        public async Task AddConfigTest(ConfigModel model)
        {
            //given
            _repositoryMock.Setup(x => x.AddConfig(It.IsAny<Config>())).ReturnsAsync(It.IsAny<int>);

            //then
            int actual = await _service.AddConfig(model);

            //when
            _repositoryMock.Verify(x => x.AddConfig(It.IsAny<Config>()), Times.Once);
        }

        [Test]
        public async Task GetAllConfigsTest()
        {
            //given
            _repositoryMock.Setup(x => x.GetAllConfigs()).ReturnsAsync(It.IsAny<List<Config>>());

            //then
            var actual = await _service.GetAllConfigs();

            //when
            _repositoryMock.Verify(x => x.GetAllConfigs(), Times.Once);
        }

        [TestCaseSource(typeof(UpdateConfigByIdTestCaseSource))]
        public async Task UpdateConfigByIdTest(int id, Config config, ConfigModel model)
        {
            //given
            _repositoryMock.Setup(x => x.GetConfigById(id)).ReturnsAsync(config);
            _repositoryMock.Setup(x => x.UpdateConfigById(id, config));
            //then
            await _service.UpdateConfigById(id, model);

            //when
            _repositoryMock.Verify(x => x.GetConfigById(id));
            _repositoryMock.Verify(x => x.UpdateConfigById(id, It.IsAny<Config>()), Times.Once);
        }

        [TestCase(3)]
        public void UpdateConfigById_WhenConfigNotFound_ShouldThrowEntityNotFoundException(int id)
        {
            //given
            _repositoryMock.Setup(x => x.UpdateConfigById(id, It.IsAny<Config>()));
            //then

            //when
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.UpdateConfigById(id, It.IsAny<ConfigModel>()));
            _repositoryMock.Verify(x => x.GetConfigById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateConfigById(It.IsAny<int>(), It.IsAny<Config>()), Times.Never);
        }

        [TestCaseSource(typeof(DeleteOrRestoreConfigTestCaseSource))]
        public async Task DeleteConfigByIdTest(Config config)
        {
            //given
            _repositoryMock.Setup(x => x.GetConfigById(It.IsAny<int>())).ReturnsAsync(config);
            _repositoryMock.Setup(x => x.DeleteOrRestoreConfigById(It.IsAny<int>(), true));

            //then
            await _service.DeleteConfigById(It.IsAny<int>());

            //when
            _repositoryMock.Verify(x => x.GetConfigById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(x => x.DeleteOrRestoreConfigById(It.IsAny<int>(), true), Times.Once);
        }

        [Test]
        public void DeleteConfigByIdTest_WhenConfigNotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            _repositoryMock.Setup(x => x.GetConfigById(It.IsAny<int>()));
            _repositoryMock.Setup(x => x.DeleteOrRestoreConfigById(It.IsAny<int>(), true));

            //then


            //when
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.DeleteConfigById(It.IsAny<int>()));
            _repositoryMock.Verify(x => x.GetConfigById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(x => x.DeleteOrRestoreConfigById(It.IsAny<int>(), true), Times.Never);
        }

        [TestCaseSource(typeof(DeleteOrRestoreConfigTestCaseSource))]
        public async Task RestoreConfigByIdTest(Config config)
        {
            //given
            _repositoryMock.Setup(x => x.GetConfigById(It.IsAny<int>())).ReturnsAsync(config);
            _repositoryMock.Setup(x => x.DeleteOrRestoreConfigById(It.IsAny<int>(), false));

            //then
            await _service.RestoreConfigById(It.IsAny<int>());

            //when
            _repositoryMock.Verify(x => x.GetConfigById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(x => x.DeleteOrRestoreConfigById(It.IsAny<int>(), false), Times.Once);
        }

        [Test]
        public void RestoreConfigByIdTest_WhenConfigNotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            _repositoryMock.Setup(x => x.GetConfigById(It.IsAny<int>()));
            _repositoryMock.Setup(x => x.DeleteOrRestoreConfigById(It.IsAny<int>(), false));

            //then


            //when
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.RestoreConfigById(It.IsAny<int>()));
            _repositoryMock.Verify(x => x.GetConfigById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(x => x.DeleteOrRestoreConfigById(It.IsAny<int>(), false), Times.Never);
        }
    }
}