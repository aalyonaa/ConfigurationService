using Marvelous.Contracts.RequestModels;
using MarvelousConfigs.BLL.AuthRequestClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarvelousConfigs.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthRequestClient _auth;

        public AuthController(ILogger<AuthController> logger, IAuthRequestClient service)
        {
            _auth = service;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation("Authentication")]
        public async Task<ActionResult<string>> Login([FromBody] AuthRequestModel auth)
        {
            _logger.LogInformation($"Trying to login with email {auth.Email}");
            var token = await _auth.GetToken(auth);
            _logger.LogInformation($"Admin with email {auth.Email} successfully logged in");
            return Ok(token.Content);
        }
    }
}

