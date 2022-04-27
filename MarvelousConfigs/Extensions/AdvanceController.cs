using AutoMapper;
using Marvelous.Contracts.Enums;
using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MarvelousConfigs.API.Extensions
{
    public class AdvanceController : ControllerBase
    {
        protected IAuthRequestClient _auth { get; set; }
        protected ILogger _logger { get; set; }
        protected IMapper _map { get; set; }

        protected AdvanceController(IAuthRequestClient auth, ILogger logger, IMapper mapper)
        {
            _auth = auth;
            _logger = logger;
            _map = mapper;
        }

        protected async Task CheckRole(params Role[] roles)
        {
            _logger.LogInformation($"Query for validation of token to {Microservice.MarvelousAuth}");
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            if (token is null)
                throw new UnauthorizedException($"Request attempt from unauthorized user");
            var lead = await _auth.SendRequestToValidateToken(token);

            if (!roles.Select(r => r.ToString()).Contains(lead.Role))
            {
                throw new ForbiddenException($"Request attempt from user with role:{lead.Role}. User doesn't have access");
            }
        }
    }
}
