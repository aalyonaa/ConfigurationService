using AutoMapper;
using MarvelousConfigs.BLL.Configuration;
using MarvelousConfigs.BLL.Exeptions;
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
    public class MicroserviceTests
    {
        private Mock<IMicroserviceRepository> _repositoryMock;
        private IMapper _map;
        private IMicroservicesService _service;
        private IMemoryCache _cache;
        private Mock<ILogger<MicroservicesService>> _logger;

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
        public async Task AddMicroserviceTest()
        {
            //given
            _repositoryMock.Setup(x => x.AddMicroservice(It.IsAny<Microservice>())).ReturnsAsync(It.IsAny<int>());

            //then
            var actual = await _service.AddMicroservice(It.IsAny<MicroserviceModel>());

            //when
            _repositoryMock.Verify(x => x.AddMicroservice(It.IsAny<Microservice>()), Times.Once);
        }

        [Test]
        public async Task GetAllMicroservicesTest()
        {
            //given
            _repositoryMock.Setup(x => x.GetAllMicroservices()).ReturnsAsync(It.IsAny<List<Microservice>>);

            //then
            var actual = await _service.GetAllMicroservices();

            //when
            _repositoryMock.Verify(x => x.GetAllMicroservices(), Times.Once);
        }

        [TestCaseSource(typeof(UpdateMicroserviceByIdTestCaseSource))]
        public async Task UpdateMicroserviceByIdTest(int id, Microservice microservice, MicroserviceModel model)
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(microservice);
            _repositoryMock.Setup(x => x.UpdateMicroserviceById(id, It.IsAny<Microservice>()));

            //then
            await _service.UpdateMicroservice(id, model);

            //when
            _repositoryMock.Verify(x => x.UpdateMicroserviceById(id, It.IsAny<Microservice>()), Times.Once);
            _repositoryMock.Verify(x => x.GetMicroserviceById(id));
        }

        [Test]
        public void UpdateMicroserviceByIdTest_WhenMicroserviceNotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(It.IsAny<int>()));
            _repositoryMock.Setup(x => x.UpdateMicroserviceById(It.IsAny<int>(), It.IsAny<Microservice>()));

            //then

            //when
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.UpdateMicroservice(It.IsAny<int>(), It.IsAny<MicroserviceModel>()));
            _repositoryMock.Verify(x => x.UpdateMicroserviceById(It.IsAny<int>(), It.IsAny<Microservice>()), Times.Never);
            _repositoryMock.Verify(x => x.GetMicroserviceById(It.IsAny<int>()), Times.Once);
        }

        [TestCaseSource(typeof(DeleteOrRestoreMicroserviceByIdTestCaseSource))]
        public async Task DeleteMicroserviceByIdTest(int id, Microservice microservice)
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(microservice);
            _repositoryMock.Setup(x => x.DeleteOrRestoreMicroserviceById(id, true));

            //then
            await _service.DeleteMicroservice(id);

            //when
            _repositoryMock.Verify(x => x.DeleteOrRestoreMicroserviceById(id, true), Times.Once);
            _repositoryMock.Verify(x => x.GetMicroserviceById(id), Times.Once);
        }

        [Test]
        public void DeleteMicroserviceByIdTest_WhenMicroserviceNotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(It.IsAny<int>()));
            _repositoryMock.Setup(x => x.DeleteOrRestoreMicroserviceById(It.IsAny<int>(), true));

            //then

            //when
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.DeleteMicroservice(It.IsAny<int>()));
            _repositoryMock.Verify(x => x.DeleteOrRestoreMicroserviceById(It.IsAny<int>(), true), Times.Never);
            _repositoryMock.Verify(x => x.GetMicroserviceById(It.IsAny<int>()), Times.Once);
        }

        [TestCaseSource(typeof(DeleteOrRestoreMicroserviceByIdTestCaseSource))]
        public async Task RestoreMicroserviceByIdTest(int id, Microservice microservice)
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(microservice);
            _repositoryMock.Setup(x => x.DeleteOrRestoreMicroserviceById(id, false));

            //then
            await _service.RestoreMicroservice(id);

            //when
            _repositoryMock.Verify(x => x.DeleteOrRestoreMicroserviceById(id, false), Times.Once);
            _repositoryMock.Verify(x => x.GetMicroserviceById(id), Times.Once);
        }

        [Test]
        public void RestoreMicroserviceByIdTest_WhenMicroserviceNotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(It.IsAny<int>()));
            _repositoryMock.Setup(x => x.DeleteOrRestoreMicroserviceById(It.IsAny<int>(), false));

            //then         

            //when
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.RestoreMicroservice(It.IsAny<int>()));
            _repositoryMock.Verify(x => x.DeleteOrRestoreMicroserviceById(It.IsAny<int>(), false), Times.Never);
            _repositoryMock.Verify(x => x.GetMicroserviceById(It.IsAny<int>()), Times.Once);
        }

        [TestCaseSource(typeof(GetMicroserviceWithConfigsByIdTestCaseSource))]
        public async Task GetMicroserviceWithConfigsByIdTest(int id, Microservice microservice)
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(microservice);
            _repositoryMock.Setup(x => x.GetMicroserviceWithConfigsById(id)).ReturnsAsync(It.IsAny<MicroserviceWithConfigs>());

            //then
            var actual = await _service.GetMicroserviceWithConfigsById(id);

            //when
            _repositoryMock.Verify(x => x.GetMicroserviceWithConfigsById(id), Times.Once);
            _repositoryMock.Verify(x => x.GetMicroserviceById(id), Times.Once);
        }

        [Test]
        public void GetMicroserviceWithConfigsByIdTest_WhenMicroserviceNotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            _repositoryMock.Setup(x => x.GetMicroserviceById(It.IsAny<int>()));
            _repositoryMock.Setup(x => x.GetMicroserviceWithConfigsById(It.IsAny<int>())).ReturnsAsync(It.IsAny<MicroserviceWithConfigs>());

            //then

            //when
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.GetMicroserviceWithConfigsById(It.IsAny<int>()));
            _repositoryMock.Verify(x => x.GetMicroserviceWithConfigsById(It.IsAny<int>()), Times.Never);
            _repositoryMock.Verify(x => x.GetMicroserviceById(It.IsAny<int>()), Times.Once);
        }
    }
}
