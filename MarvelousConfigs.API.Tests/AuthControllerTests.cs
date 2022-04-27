using FluentValidation;
using Marvelous.Contracts.RequestModels;
using MarvelousConfigs.API.Controllers;
using MarvelousConfigs.API.Models.Validation;
using MarvelousConfigs.BLL.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MarvelousConfigs.API.Tests
{
    internal class AuthControllerTests : BaseVerifyTest<AuthController>
    {
        private Mock<IAuthRequestClient> _auth;
        private AuthController _controller;
        private IValidator<AuthRequestModel> _validator;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<AuthController>>();
            _auth = new Mock<IAuthRequestClient>();
            _validator = new AuthRequestModelValidator();
            _controller = new AuthController(_logger.Object, _auth.Object, _validator);
        }

        [TestCaseSource(typeof(LoginTestCaseSource))]
        public async Task LoginTest_Should200Ok(AuthRequestModel model)
        {
            //given
            var token = "token";
            _auth.Setup(x => x.GetToken(model)).ReturnsAsync(token);

            //when
            await _controller.Login(model);

            //then
            _auth.Verify(x => x.GetToken(model), Times.Once);
            VerifyLogger(LogLevel.Information, 3);
        }

        [Test]
        public void LoginTest_WhenEmailOrPasspoworNotValid_ShouldThrowValidationException()
        {
            //given
            var token = "token";
            AuthRequestModel model = new AuthRequestModel()
            {
                Email = default,
                Password = default,
            };
            _auth.Setup(x => x.GetToken(model)).ReturnsAsync(token);

            //when

            //then
            Assert.ThrowsAsync<ValidationException>(async () => await _controller.Login(model));
            _auth.Verify(x => x.GetToken(model), Times.Never);
            VerifyLogger(LogLevel.Information, 1);
        }
    }
}
