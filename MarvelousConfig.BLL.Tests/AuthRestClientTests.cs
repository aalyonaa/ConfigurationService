using FluentValidation;
using Marvelous.Contracts.Client;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MarvelousConfigs.BLL.Tests
{
    internal class AuthRestClientTests : BaseVerifyTest<AuthRequestClient>
    {
        private IConfiguration _config;
        private Mock<IRestClient> _client;
        private IAuthRequestClient _authRequestClient;
        private const string _message = "test exception message";

        [SetUp]
        public void SetUp()
        {
            _client = new Mock<IRestClient>();
            _logger = new Mock<ILogger<AuthRequestClient>>();
            _config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()).Build();
            _config[Microservice.MarvelousAuth.ToString()] = "https://piter-education.ru:6042";
            _authRequestClient = new AuthRequestClient(_logger.Object, _config, _client.Object);
        }

        #region get token test

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public void GetTokenTest_WhenAuthServiceReturnedHttpCode401Unauthorized_ShouldThrowUnauthorizedException(AuthRequestModel auth)
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.Unauthorized && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<UnauthorizedException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public void GetTokenTest_WhenAuthServiceReturnedHttpCode403Forbidden_ShouldThrowForbiddenException(AuthRequestModel auth)
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.Forbidden && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ForbiddenException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public void GetTokenTest_WhenAuthServiceReturnedHttpCode400BadRequest_ShouldThrowBadRequestException(AuthRequestModel auth)
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.BadRequest && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<BadRequestException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public void GetTokenTest_WhenAuthServiceReturnedHttpCode404NotFound_ShouldThrowEntityNotFoundException(AuthRequestModel auth)
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.NotFound && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public void GetTokenTest_WhenAuthServiceReturnedHttpCode409Conflict_ShouldThrowConflictException(AuthRequestModel auth)
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.Conflict && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ConflictException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public void GetTokenTest_WhenAuthServiceReturnedHttpCode422UnprocessableEntity_ShouldThrowValidationException(AuthRequestModel auth)
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.UnprocessableEntity && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ValidationException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public void GetTokenTest_WhenAuthServiceReturnedHttpCode503ServiceUnavailable_ShouldThrowServiceUnavailableException(AuthRequestModel auth)
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.ServiceUnavailable && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ServiceUnavailableException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public void GetTokenTest_WhenAuthServiceReturnedNotProvidedHttpCode_ShouldThrowServiceBadGatewayException(AuthRequestModel auth)
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.Unused && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<BadGatewayException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public void GetTokenTest_WhenAuthServiceAnswerIsEmpty_ShouldThrowServiceBadGatewayException(AuthRequestModel auth)
        {
            //given
            var responce = Mock.Of<RestResponse<string>>(x => x.StatusCode == HttpStatusCode.OK);
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<BadGatewayException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceReturnedHttpCode200Ok(AuthRequestModel auth)
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode == HttpStatusCode.OK);
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when
            var actual = await _authRequestClient.GetToken(auth);

            //then
            Assert.AreEqual(token, actual);
            VerifyRequestTests<string>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        #endregion

        #region send request to validate token test

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode200Ok()
        {
            //given
            IdentityResponseModel model = new IdentityResponseModel()
            {
                Id = 1,
                Role = Role.Admin.ToString(),
                IssuerMicroservice = Microservice.MarvelousConfigs.ToString()
            };

            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == model && x.StatusCode == HttpStatusCode.OK);
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when
            var actual = await _authRequestClient.SendRequestToValidateToken(token);

            //then
            VerifyRequestTests<IdentityResponseModel>(_client);
            Assert.AreEqual(model, actual);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode401Unauthorized_ShouldThrowUnauthorizedException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Unauthorized && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<UnauthorizedException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode403Forbidden_ShouldThrowForbiddenException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Forbidden && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ForbiddenException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode400BadRequest_ShouldThrowBadRequestException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.BadRequest && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<BadRequestException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode404NotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.NotFound && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode409Conflict_ShouldThrowConflictException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Conflict && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ConflictException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode422UnprocessableEntity_ShouldThrowValidationException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.UnprocessableEntity && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ValidationException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode503ServiceUnavailable_ShouldThrowServiceUnavailableException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.ServiceUnavailable && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ServiceUnavailableException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestToValidateTokenTest_WhenAuthServiceReturnedNotProvidedHttpCode_ShouldThrowServiceBadGatewayException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.AlreadyReported && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<BadGatewayException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestToValidateTokenTest_WhenAuthServiceAnswerIsEmpty_ShouldThrowServiceBadGatewayException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == default && x.StatusCode ==
            HttpStatusCode.OK && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<BadGatewayException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        #endregion

        #region send request with token

        [Test]
        public async Task SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode200Ok()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode == HttpStatusCode.OK);
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when
            await _authRequestClient.SendRequestWithToken(token);

            //then
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode401Unauthorized_ShouldThrowUnauthorizedException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Unauthorized && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<UnauthorizedException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);

        }

        [Test]
        public void SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode403Forbidden_ShouldThrowForbiddenException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Forbidden && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ForbiddenException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode400BadRequest_ShouldThrowBadRequestException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.BadRequest && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<BadRequestException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode404NotFound_ShouldThrowEntityNotFoundException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.NotFound && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestWithTokenTest_WhenAuthServiceReturned409Conflict_ShouldThrowConflictException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Conflict && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ConflictException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode422UnprocessableEntity_ShouldThrowValidationException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.UnprocessableEntity && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ValidationException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode503ServiceUnavailable_ShouldThrowServiceUnavailableException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.ServiceUnavailable && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<ServiceUnavailableException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        [Test]
        public void SendRequestWithTokenTest_WhenAuthServiceReturnedNotProvidedHttpCode_ShouldThrowServiceBadGatewayException()
        {
            //given
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.BadGateway && x.ErrorException == new Exception(_message));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            //when

            //then
            Assert.ThrowsAsync<BadGatewayException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
            VerifyLogger(LogLevel.Information, 2);
        }

        #endregion
    }
}
