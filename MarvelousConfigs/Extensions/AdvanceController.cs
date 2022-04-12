﻿using Marvelous.Contracts.Enums;
using MarvelousConfigs.BLL.AuthRequestClient;
using MarvelousConfigs.BLL.Helper.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MarvelousConfigs.API.Extensions
{
    public class AdvanceController : ControllerBase
    {
        public IAuthRequestClient _auth { get; set; }
        public ILogger _logger { get; set; }

        public AdvanceController(IAuthRequestClient auth, ILogger logger)
        {
            _auth = auth;
            _logger = logger;
        }

        protected async Task CheckRole(params Role[] roles)
        {
            _logger.LogInformation($"Query for validation of token to {Microservice.MarvelousAuth}");
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            if (token is null)
                throw new ForbiddenException($"User not authenticated");
            var lead = await _auth.SendRequestToValidateToken(token);
            if (!roles.Select(r => r.ToString()).Contains(lead.Data!.Role))
            {
                throw new ForbiddenException($"User with role:{lead.Data!.Role} don't have acces to this method");
            }
        }
    }
}
