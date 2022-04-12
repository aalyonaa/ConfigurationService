using AutoMapper;
using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.API.Extensions;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.API.RMQ.Producers;
using MarvelousConfigs.BLL.AuthRequestClient;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarvelousConfigs.API.Controllers
{
    [ApiController]
    [Route("api/configs")]
    public class ConfigsController : AdvanceController
    {
        private readonly IConfigsService _service;
        private readonly IMapper _map;
        private readonly ILogger<ConfigsController> _logger;
        private readonly IMarvelousConfigsProducer _prod;
        private readonly IAuthRequestClient _auth;

        public ConfigsController(IMapper mapper, IConfigsService service,
            ILogger<ConfigsController> logger, IMarvelousConfigsProducer producer, IAuthRequestClient auth) : base(auth, logger)
        {
            _map = mapper;
            _service = service;
            _logger = logger;
            _prod = producer;
            _auth = auth;
        }

        //api/configs
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Get all configs")]
        public async Task<ActionResult<List<ConfigOutputModel>>> GetAllConfigs()
        {
            await CheckRole(Role.Admin);
            _logger.LogInformation($"Request to get all configs");
            List<ConfigOutputModel>? configs = _map.Map<List<ConfigOutputModel>>(await _service.GetAllConfigs());
            _logger.LogInformation($"Response to a request for all configs");
            return Ok(configs);
        }

        //api/configs/42
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Update config by id")]
        public async Task<ActionResult> UpdateConfigById(int id, [FromBody] ConfigInputModel model)
        {
            await CheckRole(Role.Admin);
            _logger.LogInformation($"Request to update config by id{id}");
            await _service.UpdateConfigById(id, _map.Map<ConfigModel>(model));
            await _prod.NotifyConfigurationUpdated(id);
            _logger.LogInformation($"Response to a request for update config by id{id}");
            return NoContent();
        }

        //api/configs/service/42
        [HttpGet("service/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Get configs by service id")]
        public async Task<ActionResult<List<ConfigOutputModel>>> GetConfigsByServiceId(int id)
        {
            await CheckRole(Role.Admin);
            _logger.LogInformation($"Request to get configs by service id{id}");
            List<ConfigOutputModel>? configs = _map.Map<List<ConfigOutputModel>>(await _service.GetConfigsByServiceId(id));
            _logger.LogInformation($"Response to a request for get configs by service id{id}");
            return Ok(configs);
        }

        //api/configs/by-service
        [HttpGet(ConfigsEndpoints.Configs)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Get configs by service address")]
        public async Task<ActionResult<List<ConfigResponseModel>>> GetConfigsByService()
        {

            _logger.LogInformation($"Request to get configs by service");
            Microsoft.Extensions.Primitives.StringValues a = HttpContext.Request.Headers.Authorization;
            string? name = HttpContext.Request.Headers[nameof(Microservice)][0];
            _logger.LogInformation($"Call belongs to the service {$"{name}"}");
            List<ConfigResponseModel>? configs = _map.Map<List<ConfigResponseModel>>(await _service.GetConfigsByService(a, name));
            _logger.LogInformation($"Response to a request for get configs by service {name}");
            return Ok(configs);
        }

    }
}
