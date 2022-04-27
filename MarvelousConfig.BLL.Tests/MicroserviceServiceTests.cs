using AutoMapper;
using MarvelousConfigs.BLL.Configuration;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using MarvelousConfigs.DAL;
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
    internal class MicroserviceServiceTests : BaseVerifyTest<MicroservicesService>
    {
        private Mock<IMicroserviceRepository> _repositoryMock;
        private IMicroservicesService _service;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IMicroserviceRepository>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _logger = new Mock<ILogger<MicroservicesService>>();
            _map = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CustomMapperBLL>()));
            _service = new MicroservicesService(_repositoryMock.Object, _map, _cache, _logger.Object);
        }

        [Test]
        public async Task GetAllMicroservicesTest()
        {
            //given
            _repositoryMock.Setup(x => x.GetAllMicroservices()).ReturnsAsync(It.IsAny<List<Microservice>>);

            //when
            List<MicroserviceModel>? actual = await _service.GetAllMicroservices();

            //then
            _repositoryMock.Verify(x => x.GetAllMicroservices(), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCase(2)]
        public async Task GetMicroserviceByIdTest(int id)
        {
            //given
            Microservice microservice = new Microservice() { Id = 1, ServiceName = "Name", Address = "123456", Url = "12345" };
            _repositoryMock.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(microservice);

            //when
            await _service.GetMicroserviceById(id);

            //then
            _repositoryMock.Verify(x => x.GetMicroserviceById(id), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void GetMicroserviceByIdTest_WhenMicroserviceNotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(It.IsAny<int>()));

            //when

            //then
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.GetMicroserviceById(It.IsAny<int>()));
            _repositoryMock.Verify(x => x.GetMicroserviceById(It.IsAny<int>()), Times.Once);
        }

        [TestCaseSource(typeof(UpdateMicroserviceByIdTestCaseSource))]
        public async Task UpdateMicroserviceByIdTest(int id, Microservice microservice, MicroserviceModel model)
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(microservice);
            _repositoryMock.Setup(x => x.UpdateMicroserviceById(id, It.IsAny<Microservice>()));

            //when
            await _service.UpdateMicroservice(id, model);

            //then
            _repositoryMock.Verify(x => x.UpdateMicroserviceById(id, It.IsAny<Microservice>()), Times.Once);
            _repositoryMock.Verify(x => x.GetMicroserviceById(id));
            VerifyLogger(LogLevel.Information, 4);
        }

        [Test]
        public void UpdateMicroserviceByIdTest_WhenMicroserviceNotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(It.IsAny<int>()));
            _repositoryMock.Setup(x => x.UpdateMicroserviceById(It.IsAny<int>(), It.IsAny<Microservice>()));

            //when

            //then
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.UpdateMicroservice(It.IsAny<int>(), It.IsAny<MicroserviceModel>()));
            _repositoryMock.Verify(x => x.UpdateMicroserviceById(It.IsAny<int>(), It.IsAny<Microservice>()), Times.Never);
            _repositoryMock.Verify(x => x.GetMicroserviceById(It.IsAny<int>()), Times.Once);
        }

        [TestCaseSource(typeof(GetMicroserviceWithConfigsByIdTestCaseSource))]
        public async Task GetMicroserviceWithConfigsByIdTest(int id, Microservice microservice)
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(microservice);
            _repositoryMock.Setup(x => x.GetMicroserviceWithConfigsById(id)).ReturnsAsync(It.IsAny<MicroserviceWithConfigs>());

            //when
            MicroserviceWithConfigsModel? actual = await _service.GetMicroserviceWithConfigsById(id);

            //then
            _repositoryMock.Verify(x => x.GetMicroserviceWithConfigsById(id), Times.Once);
            _repositoryMock.Verify(x => x.GetMicroserviceById(id), Times.Once);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void GetMicroserviceWithConfigsByIdTest_WhenMicroserviceNotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(It.IsAny<int>()));
            _repositoryMock.Setup(x => x.GetMicroserviceWithConfigsById(It.IsAny<int>())).ReturnsAsync(It.IsAny<MicroserviceWithConfigs>());

            //when

            //then
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.GetMicroserviceWithConfigsById(It.IsAny<int>()));
            _repositoryMock.Verify(x => x.GetMicroserviceWithConfigsById(It.IsAny<int>()), Times.Never);
            _repositoryMock.Verify(x => x.GetMicroserviceById(It.IsAny<int>()), Times.Once);
        }
    }
}
