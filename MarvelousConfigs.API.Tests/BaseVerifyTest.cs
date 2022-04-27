using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using static Moq.It;

namespace MarvelousConfigs.API.Tests
{
    internal class BaseVerifyTest<T>
    {
        protected Mock<ILogger<T>> _logger;
        protected IMapper _map;

        protected void VerifyLogger(LogLevel level, int times)
        {
            _logger.Verify(x => x.Log(level,
                    It.IsAny<EventId>(),
                    It.Is<IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<IsAnyType, Exception, string>>()!),
                Times.Exactly(times));
        }
    }
}
