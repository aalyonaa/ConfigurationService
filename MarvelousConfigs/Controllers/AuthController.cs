using FluentValidation;
using Marvelous.Contracts.RequestModels;
using MarvelousConfigs.BLL.Infrastructure;
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
        private readonly IAuthRequestClient _auth;
        private readonly ILogger<AuthController> _logger;
        private readonly IValidator<AuthRequestModel> _validator;

        public AuthController(ILogger<AuthController> logger, IAuthRequestClient service, IValidator<AuthRequestModel> validator)
        {
            _auth = service;
            _logger = logger;
            _validator = validator;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation("Authentication")]
        public async Task<ActionResult<string>> Login([FromBody] AuthRequestModel auth)
        {
            _logger.LogInformation($"Trying to login");
            if (auth == null)
                throw new Exception("You must specify the table details in the request body");
            var validationResult = _validator.Validate(auth);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            _logger.LogInformation($"Call belongs to user with email {auth.Email}");
            var token = await _auth.GetToken(auth);
            _logger.LogInformation($"Admin with email {auth.Email} successfully logged in");
            return Ok(token);
        }
    }
}

