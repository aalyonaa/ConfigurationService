using AutoMapper;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.API.Configuration;
using MarvelousConfigs.API.Controllers;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarvelousConfigs.API.Tests
{
    internal class MicroservicesControllerTests : BaseVerifyTest<MicroservicesController>
    {
        private Mock<IMicroservicesService> _service;
        private Mock<IAuthRequestClient> _auth;
        private MicroservicesController _controller;

        [SetUp]
        public void Setup()
        {
            _map = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CustomMapperAPI>()));
            _service = new Mock<IMicroservicesService>();
            _logger = new Mock<ILogger<MicroservicesController>>();
            _auth = new Mock<IAuthRequestClient>();
            _controller = new MicroservicesController(_map, _service.Object, _auth.Object, _logger.Object);
        }

        [TestCaseSource(typeof(GetAllMicroservicesTestCaseSource))]
        public async Task GetAllMicroservicesTest_Should200Ok(List<MicroserviceModel> services, IdentityResponseModel model)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.GetAllMicroservices()).ReturnsAsync(services);

            //when
            var result = await _controller.GetAllMicroservices();

            //then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _service.Verify(x => x.GetAllMicroservices(), Times.Once);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 3);
        }

        [TestCaseSource(typeof(GetAllMicroservicesShould403TestCaseSource))]
        public void GetAllMicroservicesTest_Should403Forbidden(List<MicroserviceModel> services, IdentityResponseModel model)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;

            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);

            _service.Setup(x => x.GetAllMicroservices()).ReturnsAsync(services);

            //when

            //then
            Assert.ThrowsAsync<ForbiddenException>(async () => await _controller.GetAllMicroservices());
            _service.Verify(x => x.GetAllMicroservices(), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(GetAllMicroservicesShould401TestCaseSource))]
        public void GetAllMicroservicesTest_Should401Unauthorized(List<MicroserviceModel> services, IdentityResponseModel model)
        {
            //given
            var context = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.GetAllMicroservices()).ReturnsAsync(services);

            //when

            //then
            Assert.ThrowsAsync<UnauthorizedException>(async () => await _controller.GetAllMicroservices());
            _service.Verify(x => x.GetAllMicroservices(), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Never);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(UpdateMicroserviceTestCaseSource))]
        public async Task UpdateMicroserviceTest_Should204NoContent(MicroserviceInputModel input,
            IdentityResponseModel model, int id)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.UpdateMicroservice(id, It.IsAny<MicroserviceModel>()));

            //when
            await _controller.UpdateMicroserviceById(id, input);

            //then
            _service.Verify(x => x.UpdateMicroservice(id, It.IsAny<MicroserviceModel>()), Times.Once);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 3);
        }

        [TestCaseSource(typeof(UpdateMicroserviceShould403TestCaseSource))]
        public void UpdateMicroserviceTest_Should403Forbidden(MicroserviceInputModel input,
            IdentityResponseModel model, int id)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.UpdateMicroservice(id, It.IsAny<MicroserviceModel>()));

            //when

            //then
            Assert.ThrowsAsync<ForbiddenException>(async () => await _controller.UpdateMicroserviceById(id, input));
            _service.Verify(x => x.UpdateMicroservice(id, It.IsAny<MicroserviceModel>()), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(UpdateMicroserviceShould401TestCaseSource))]
        public void UpdateMicroserviceTest_Should401Unauthorized(MicroserviceInputModel input,
            IdentityResponseModel model, int id)
        {
            //given
            var context = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.UpdateMicroservice(id, It.IsAny<MicroserviceModel>()));

            //when

            //then
            Assert.ThrowsAsync<UnauthorizedException>(async () => await _controller.UpdateMicroserviceById(id, input));
            _service.Verify(x => x.UpdateMicroservice(id, It.IsAny<MicroserviceModel>()), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Never);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(GetMicroserviceWithConfigsByIdTestCaseSource))]
        public async Task GetMicroserviceWithConfigsByIdTest_Should200Ok(MicroserviceWithConfigsModel service, IdentityResponseModel model,
            int id)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.GetMicroserviceWithConfigsById(id)).ReturnsAsync(service);

            //when
            var result = await _controller.GetMicroserviceWithConfigsById(id);

            //then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _service.Verify(x => x.GetMicroserviceWithConfigsById(id), Times.Once);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 3);
        }

        [TestCaseSource(typeof(GetMicroserviceWithConfigsByIdShould403TestCaseSource))]
        public void GetMicroserviceWithConfigsByIdTest_Should403Forbidden(MicroserviceWithConfigsModel service, IdentityResponseModel model,
            int id)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.GetMicroserviceWithConfigsById(id)).ReturnsAsync(service);

            //when

            //then
            Assert.ThrowsAsync<ForbiddenException>(async () => await _controller.GetMicroserviceWithConfigsById(id));
            _service.Verify(x => x.GetMicroserviceWithConfigsById(id), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(GetMicroserviceWithConfigsByIdShould401TestCaseSource))]
        public void GetMicroserviceWithConfigsByIdTest_Should401Unauthorized(MicroserviceWithConfigsModel service, IdentityResponseModel model,
            int id)
        {
            //given
            var context = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.GetMicroserviceWithConfigsById(id)).ReturnsAsync(service);

            //when

            //then
            Assert.ThrowsAsync<UnauthorizedException>(async () => await _controller.GetMicroserviceWithConfigsById(id));
            _service.Verify(x => x.GetMicroserviceWithConfigsById(id), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Never);
            VerifyLogger(LogLevel.Information, 1);
        }
    }
}
