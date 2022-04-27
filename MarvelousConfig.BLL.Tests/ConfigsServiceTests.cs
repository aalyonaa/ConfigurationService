using AutoMapper;
using MarvelousConfigs.BLL.Configuration;
using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using MarvelousConfigs.BLL.Tests;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarvelousConfig.BLL.Tests
{
    internal class ConfigsServiceTests : BaseVerifyTest<ConfigsService>
    {
        private Mock<IConfigsRepository> _repositoryMock;
        private IConfigsService _service;
        private Mock<IAuthRequestClient> _auth;
        private Mock<IMemoryCacheExtentions> _memory;
        private Mock<IMarvelousConfigsProducer> _producer;

        [SetUp]
        public void Setup()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _repositoryMock = new Mock<IConfigsRepository>();
            _logger = new Mock<ILogger<ConfigsService>>();
            _auth = new Mock<IAuthRequestClient>();
            _memory = new Mock<IMemoryCacheExtentions>();
            _producer = new Mock<IMarvelousConfigsProducer>();
            _map = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CustomMapperBLL>()));
            _service = new ConfigsService(_repositoryMock.Object, _map, _cache, _memory.Object,
                _logger.Object, _auth.Object, _producer.Object);

        }

        [Test]
        public async Task GetAllConfigsTest()
        {
            //given
            _repositoryMock.Setup(x => x.GetAllConfigs()).ReturnsAsync(It.IsAny<List<Config>>());

            //then
            List<ConfigModel>? actual = await _service.GetAllConfigs();

            //when
            _repositoryMock.Verify(x => x.GetAllConfigs(), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCaseSource(typeof(UpdateConfigByIdTestCaseSource))]
        public async Task UpdateConfigByIdTest(int id, Config config, ConfigModel model)
        {
            //given
            _repositoryMock.Setup(x => x.GetConfigById(id)).ReturnsAsync(config);
            _repositoryMock.Setup(x => x.UpdateConfigById(id, config));
            _producer.Setup(x => x.NotifyConfigurationUpdated(config));

            //when
            await _service.UpdateConfigById(id, model);

            //then
            VerifyLogger(LogLevel.Information, 4);
            _repositoryMock.Verify(x => x.GetConfigById(id));
            _producer.Verify(x => x.NotifyConfigurationUpdated(config), Times.Once);
            _repositoryMock.Verify(x => x.UpdateConfigById(id, It.IsAny<Config>()), Times.Once);
        }

        [TestCase(3)]
        public void UpdateConfigById_WhenConfigNotFound_ShouldThrowEntityNotFoundException(int id)
        {
            //given
            _repositoryMock.Setup(x => x.UpdateConfigById(id, It.IsAny<Config>()));

            //when

            //then
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.UpdateConfigById(id, It.IsAny<ConfigModel>()));
            _repositoryMock.Verify(x => x.GetConfigById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateConfigById(It.IsAny<int>(), It.IsAny<Config>()), Times.Never);
        }

        [TestCase(1)]
        public async Task GetConfigByIdTest(int id)
        {
            //given
            Config config = new Config() { Id = 1, Key = "Key", Value = "Value", Created = System.DateTime.Now, ServiceId = 3 };
            _repositoryMock.Setup(x => x.GetConfigById(id)).ReturnsAsync(config);

            //then
            ConfigModel? actual = await _service.GetConfigById(id);

            //when
            _repositoryMock.Verify(x => x.GetConfigById(id), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCase(1)]
        public async Task GetConfigByIdTest_WhenConfigNotFound_ShouldThrowEntityNotFoundException(int id)
        {
            //given
            _repositoryMock.Setup(x => x.GetConfigById(id));

            //then

            //when
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.GetConfigById(id));
            _repositoryMock.Verify(x => x.GetConfigById(id), Times.Once);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCase(1)]
        public async Task GetConfigsByServiceIdTest(int id)
        {
            //given
            List<Config> configs = new List<Config>() { new Config() { Id = 1, Key = "Key", Value = "Value", Created = System.DateTime.Now, ServiceId = 3 } };
            _repositoryMock.Setup(x => x.GetConfigsByServiceId(id)).ReturnsAsync(configs);

            //then
            List<ConfigModel> actual = await _service.GetConfigsByServiceId(id);

            //when
            _repositoryMock.Verify(x => x.GetConfigsByServiceId(id), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCase("name", "token")]
        public async Task GetConfigsByServiceTest(string name, string token)
        {
            //given
            List<Config> configs = new List<Config>() { new Config() { Id = 1, Key = "Key", Value = "Value", Created = System.DateTime.Now, ServiceId = 3 } };
            _repositoryMock.Setup(x => x.GetConfigsByService(name)).ReturnsAsync(configs);
            _auth.Setup(x => x.SendRequestWithToken(token)).Returns(Task.CompletedTask);

            //then
            List<ConfigModel> actual = await _service.GetConfigsByService(token, name);

            //when
            _repositoryMock.Verify(x => x.GetConfigsByService(name), Times.Once);
            _auth.Verify(x => x.SendRequestWithToken(token), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCase("name", "token")]
        public async Task GetConfigsByServiceTest_WhenTokenIsNotValid_ShouldThrowForbiddenException(string name, string token)
        {
            //given
            _repositoryMock.Setup(x => x.GetConfigsByService(name));
            _auth.Setup(x => x.SendRequestWithToken(token)).Returns(Task.FromException(new Exception(It.IsAny<string>())));

            //then

            //when
            Assert.ThrowsAsync<Exception>(async () => await _service.GetConfigsByService(token, name));
            _repositoryMock.Verify(x => x.GetConfigsByService(name), Times.Never);
            _auth.Verify(x => x.SendRequestWithToken(token), Times.Once);
        }
    }
}