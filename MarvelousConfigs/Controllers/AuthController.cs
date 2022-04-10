using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.Services;
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
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService service)
        {
            _authService = service;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation("Authentication")]
        public ActionResult<string> Login([FromBody] AdminLoginInputModel auth)
        {
            _logger.LogInformation($"Trying to login with email {auth.Email}");
            var token = _authService.GetToken(auth.Email, auth.Password);
            _logger.LogInformation($"Admin with email {auth.Email} successfully logged in");
            return Json(token);
        }
    }
}

