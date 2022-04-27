using AutoMapper;
using Marvelous.Contracts.Enums;
using MarvelousConfigs.API.Extensions;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarvelousConfigs.API.Controllers
{
    [ApiController]
    [Route("api/microservices")]
    public class MicroservicesController : AdvanceController
    {
        private readonly IMicroservicesService _service;

        public MicroservicesController(
            IMapper mapper,
            IMicroservicesService service,
            IAuthRequestClient auth,
            ILogger<MicroservicesController> logger) : base(auth, logger, mapper)
        {
            _map = mapper;
            _service = service;
            _logger = logger;
            _auth = auth;
        }

        //api/microservices
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation("Get all microservices")]
        public async Task<ActionResult<List<MicroserviceOutputModel>>> GetAllMicroservices()
        {
            await CheckRole(Role.Admin);
            _logger.LogInformation($"Request to get all microservices");
            List<MicroserviceOutputModel>? services = _map.Map<List<MicroserviceOutputModel>>(await _service.GetAllMicroservices());
            _logger.LogInformation($"Response to a request for get all microservices");
            return Ok(services);
        }

        //api/microservices/42
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation("Update microservice by id")]
        public async Task<ActionResult> UpdateMicroserviceById(int id, [FromBody] MicroserviceInputModel model)
        {
            await CheckRole(Role.Admin);
            _logger.LogInformation($"Request to update microservice by id{id}");
            await _service.UpdateMicroservice(id, _map.Map<MicroserviceModel>(model));
            _logger.LogInformation($"Response to a request for update microservice by id{id}");
            return NoContent();
        }

        //api/microservices/42
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation("Get microservices with configs by id")]
        public async Task<ActionResult<MicroserviceWithConfigsOutputModel>> GetMicroserviceWithConfigsById(int id)
        {
            await CheckRole(Role.Admin);
            _logger.LogInformation($"Request to get microservice with configs by id{id}");
            MicroserviceWithConfigsOutputModel? services = _map.Map<MicroserviceWithConfigsOutputModel>(await _service.GetMicroserviceWithConfigsById(id));
            _logger.LogInformation($"Response to a request for get microservice with configs by id{id}");
            return Ok(services);
        }
    }
}
