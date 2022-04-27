using AutoMapper;
using FluentValidation;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.API.Configuration;
using MarvelousConfigs.API.Controllers;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.API.Models.Validation;
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
    internal class ConfigsControllerTests : BaseVerifyTest<ConfigsController>
    {
        private IValidator<ConfigInputModel> _validator;
        private Mock<IConfigsService> _service;
        private Mock<IAuthRequestClient> _auth;
        private ConfigsController _controller;

        [SetUp]
        public void Setup()
        {
            _map = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CustomMapperAPI>()));
            _service = new Mock<IConfigsService>();
            _logger = new Mock<ILogger<ConfigsController>>();
            _validator = new ConfigInputModelValidator();
            _auth = new Mock<IAuthRequestClient>();
            _controller = new ConfigsController(_map, _service.Object, _logger.Object, _auth.Object, _validator);
        }

        [TestCaseSource(typeof(GetAllConfigsTestCaseSource))]
        public async Task GetAllConfigsTest_Should200Ok(List<ConfigModel> configs, IdentityResponseModel model)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.GetAllConfigs()).ReturnsAsync(configs);

            //when
            var result = await _controller.GetAllConfigs();

            //then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _service.Verify(x => x.GetAllConfigs(), Times.Once);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 3);
        }

        [TestCaseSource(typeof(GetAllConfigsShould403TestCaseSource))]
        public void GetAllConfigsTest_Should403Forbidden(List<ConfigModel> configs, IdentityResponseModel model)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;

            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);

            _service.Setup(x => x.GetAllConfigs()).ReturnsAsync(configs);

            //when

            //then
            Assert.ThrowsAsync<ForbiddenException>(async () => await _controller.GetAllConfigs());
            _service.Verify(x => x.GetAllConfigs(), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(GetAllConfigsShould401TestCaseSource))]
        public void GetAllConfigsTest_Should401Unauthorized(List<ConfigModel> configs, IdentityResponseModel model)
        {
            //given
            var context = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.GetAllConfigs()).ReturnsAsync(configs);

            //when

            //then
            Assert.ThrowsAsync<UnauthorizedException>(async () => await _controller.GetAllConfigs());
            _service.Verify(x => x.GetAllConfigs(), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Never);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(UpdateConfigTestCaseSource))]
        public async Task UpdateConfigTest_Should204NoContent(ConfigInputModel input,
            IdentityResponseModel model, int id)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.UpdateConfigById(id, It.IsAny<ConfigModel>()));

            //when
            await _controller.UpdateConfigById(id, input);

            //then
            _service.Verify(x => x.UpdateConfigById(id, It.IsAny<ConfigModel>()), Times.Once);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 3);
        }

        [TestCaseSource(typeof(UpdateConfigShould403TestCaseSource))]
        public void UpdateConfigTest_Should403Forbidden(ConfigInputModel input,
            IdentityResponseModel model, int id)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.UpdateConfigById(id, It.IsAny<ConfigModel>()));

            //when

            //then
            Assert.ThrowsAsync<ForbiddenException>(async () => await _controller.UpdateConfigById(id, input));
            _service.Verify(x => x.UpdateConfigById(id, It.IsAny<ConfigModel>()), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(UpdateConfigShould401TestCaseSource))]
        public void UpdateConfigTest_Should401Unauthorized(ConfigInputModel input,
            IdentityResponseModel model, int id)
        {
            //given
            var context = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.UpdateConfigById(id, It.IsAny<ConfigModel>()));

            //when

            //then
            Assert.ThrowsAsync<UnauthorizedException>(async () => await _controller.UpdateConfigById(id, input));
            _service.Verify(x => x.UpdateConfigById(id, It.IsAny<ConfigModel>()), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Never);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(GetConfigsByServiceIdTestCaseSource))]
        public async Task GetConfigsByServiceIdTest_Should200Ok(List<ConfigModel> configs, IdentityResponseModel model,
            int id)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.GetConfigsByServiceId(id)).ReturnsAsync(configs);

            //when
            var result = await _controller.GetConfigsByServiceId(id);

            //then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _service.Verify(x => x.GetConfigsByServiceId(id), Times.Once);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 3);
        }

        [TestCaseSource(typeof(GetConfigsByServiceIdShould403TestCaseSource))]
        public void GetConfigsByServiceIdTest_Should403Forbidden(List<ConfigModel> configs, IdentityResponseModel model,
            int id)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.GetConfigsByServiceId(id)).ReturnsAsync(configs);

            //when

            //then
            Assert.ThrowsAsync<ForbiddenException>(async () => await _controller.GetConfigsByServiceId(id));
            _service.Verify(x => x.GetConfigsByServiceId(id), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(GetConfigsByServiceIdShould401TestCaseSource))]
        public void GetConfigsByServiceIdTest_Should401Unauthorized(List<ConfigModel> configs, IdentityResponseModel model,
            int id)
        {
            //given
            var context = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext = context;
            _auth.Setup(x => x.SendRequestToValidateToken(It.IsAny<string>())).ReturnsAsync(model);
            _service.Setup(x => x.GetConfigsByServiceId(id)).ReturnsAsync(configs);

            //when

            //then
            Assert.ThrowsAsync<UnauthorizedException>(async () => await _controller.GetConfigsByServiceId(id));
            _service.Verify(x => x.GetConfigsByServiceId(id), Times.Never);
            _auth.Verify(x => x.SendRequestToValidateToken(It.IsAny<string>()), Times.Never);
            VerifyLogger(LogLevel.Information, 1);
        }

        [TestCaseSource(typeof(GetConfigsByServiceTestCaseSource))]
        public async Task GetConfigsByServiceTest_Should200Ok(List<ConfigModel> configs, Microservice microservice)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            context.Request.Headers.Add(nameof(Microservice), microservice.ToString());
            _controller.ControllerContext.HttpContext = context;
            _service.Setup(x => x.GetConfigsByService(token, microservice.ToString())).ReturnsAsync(configs);

            //when
            var result = await _controller.GetConfigsByService();

            //then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            _service.Verify(x => x.GetConfigsByService(token, microservice.ToString()), Times.Once);
            VerifyLogger(LogLevel.Information, 3);
        }

        [Test]
        public void GetConfigsByServiceTest_Should401Unauthorized()
        {
            //given
            var context = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext = context;
            _service.Setup(x => x.GetConfigsByService(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<List<ConfigModel>>());

            //when

            //then
            Assert.ThrowsAsync<UnauthorizedException>(async () => await _controller.GetConfigsByService());
            _service.Verify(x => x.GetConfigsByService(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            VerifyLogger(LogLevel.Information, 1);
        }
    }
}