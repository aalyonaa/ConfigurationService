using FluentValidation;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.API.Infrastructure;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MarvelousConfigs.API.Tests
{
    internal class GlobalExceptionHandlerTests : BaseVerifyTest<GlobalExceptionHandler>
    {
        private DefaultHttpContext _context;
        private const string _message = "test exception _message";

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<GlobalExceptionHandler>>();
            _context = new DefaultHttpContext
            {
                Response = { Body = new MemoryStream() },
                Request = { Path = "/" }
            };
        }

        [Test]
        public async Task InvokeAsyncTest_ValidRequestReceived_ShouldResponse()
        {
            //given
            const string expected = "Request handed over to next request delegate";
            var exceptionHandler = new GlobalExceptionHandler(innerHttpContext =>
            {
                innerHttpContext.Response.WriteAsync(expected);
                return Task.CompletedTask;
            }, _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 0);
        }

        [Test]
        public async Task InvokeAsyncTest_WhenThrowCacheLoadingException_ShouldExceptionResponseModelWithCode502()
        {
            //given
            var expected = GetJsonExceptionResponseModel(502);
            var exceptionHandler = new GlobalExceptionHandler(_ => throw new CacheLoadingException(_message), _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 1);
        }

        [Test]
        public async Task InvokeAsyncTest_WhenThrowUnauthorizedException_ShouldExceptionResponseModelWithCode401()
        {
            //given
            var expected = GetJsonExceptionResponseModel(401);
            var exceptionHandler = new GlobalExceptionHandler(_ => throw new UnauthorizedException(_message), _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 1);
        }

        [Test]
        public async Task InvokeAsyncTest_WhenThrowForbiddenException_ShouldExceptionResponseModelWithCode403()
        {
            //given
            var expected = GetJsonExceptionResponseModel(403);
            var exceptionHandler = new GlobalExceptionHandler(_ => throw new ForbiddenException(_message), _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 1);
        }

        [Test]
        public async Task InvokeAsyncTest_WhenThrowEntityNotFoundException_ShouldExceptionResponseModelWithCode404()
        {
            //given
            var expected = GetJsonExceptionResponseModel(404);
            var exceptionHandler = new GlobalExceptionHandler(_ => throw new EntityNotFoundException(_message), _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 1);
        }

        [Test]
        public async Task InvokeAsyncTest_WhenThrowValidationException_ShouldExceptionResponseModelWithCode422()
        {
            //given
            var expected = GetJsonExceptionResponseModel(422);
            var exceptionHandler = new GlobalExceptionHandler(_ => throw new ValidationException(_message), _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 1);
        }

        [Test]
        public async Task InvokeAsyncTest_WhenThrowBadGatewayException_ShouldExceptionResponseModelWithCode502()
        {
            //given
            var expected = GetJsonExceptionResponseModel(502);
            var exceptionHandler = new GlobalExceptionHandler(_ => throw new BadGatewayException(_message), _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 1);
        }

        [Test]
        public async Task InvokeAsyncTest_WhenThrowBadRequestException_ShouldExceptionResponseModelWithCode400()
        {
            //given
            var expected = GetJsonExceptionResponseModel(400);
            var exceptionHandler = new GlobalExceptionHandler(_ => throw new BadRequestException(_message), _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 1);
        }

        [Test]
        public async Task InvokeAsyncTest_WhenThrowConflictException_ShouldExceptionResponseModelWithCode409()
        {
            //given
            var expected = GetJsonExceptionResponseModel(409);
            var exceptionHandler = new GlobalExceptionHandler(_ => throw new ConflictException(_message), _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 1);
        }

        [Test]
        public async Task InvokeAsyncTest_WhenThrowServiceUnavailableException_ShouldExceptionResponseModelWithCode503()
        {
            //given
            var expected = GetJsonExceptionResponseModel(503);
            var exceptionHandler = new GlobalExceptionHandler(_ => throw new ServiceUnavailableException(_message), _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 1);
        }

        [Test]
        public async Task InvokeAsyncTest_WhenThrowException_ShouldExceptionResponseModelWithCode400()
        {
            //given
            var expected = GetJsonExceptionResponseModel(400);
            var exceptionHandler = new GlobalExceptionHandler(_ => throw new Exception(_message), _logger.Object);

            //when
            await exceptionHandler.InvokeAsync(_context);

            //then
            var actual = GetResponseBody();
            Assert.AreEqual(expected, actual);
            VerifyLogger(LogLevel.Error, 1);
        }

        private static string GetJsonExceptionResponseModel(int statusCode) =>
            JsonSerializer.Serialize(new ExceptionResponseModel { Code = statusCode, Message = _message });

        private string GetResponseBody()
        {
            _context.Response.Body.Seek(0, SeekOrigin.Begin);
            return new StreamReader(_context.Response.Body).ReadToEnd();
        }
    }
}
