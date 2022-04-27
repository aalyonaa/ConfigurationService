using AutoMapper;
using Marvelous.Contracts.Client;
using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using RestSharp;
using System;
using static Moq.It;

namespace MarvelousConfigs.BLL.Tests
{
    internal class BaseVerifyTest<T>
    {
        protected Mock<ILogger<T>> _logger;
        protected IMapper _map;
        protected IMemoryCache _cache;

        protected void VerifyLogger(LogLevel level, int times)
        {
            _logger.Verify(x => x.Log(level,
                    It.IsAny<EventId>(),
                    It.Is<IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<IsAnyType, Exception, string>>()!),
                Times.Exactly(times));
        }

        protected static void VerifyRequestTests<T>(Mock<IRestClient> client)
        {
            client.Verify(x => x.AddMicroservice(Microservice.MarvelousConfigs), Times.Once);
            client.Verify(x => x.ExecuteAsync<T>(It.IsAny<RestRequest>(), default), Times.Once);
        }
    }
}
