using AutoMapper;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarvelousConfigs.API.Controllers
{
    [ApiController]
    //[AuthorizeEnum(Role.Admin)]
    [Route("api/microservices")]
    public class MicroservicesController : ControllerBase
    {
        private readonly IMicroservicesService _service;
        private readonly IMapper _map;
        private readonly ILogger<MicroservicesController> _logger;

        public MicroservicesController(IMapper mapper, IMicroservicesService service, ILogger<MicroservicesController> logger)
        {
            _map = mapper;
            _service = service;
            _logger = logger;
        }

        //api/microservices
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Add microservice")]
        public async Task<ActionResult<int>> AddMicroservice([FromBody] MicroserviceInputModel model)
        {
            _logger.LogInformation($"Request to add new microservice");
            int id = await _service.AddMicroservice(_map.Map<MicroserviceModel>(model));
            _logger.LogInformation($"Response to a request for add new microservice id {id}");
            return StatusCode(StatusCodes.Status201Created, id);
        }

        //api/microservices/42
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Delete microservice by id")]
        public async Task<ActionResult> DeleteMicroserviceById(int id)
        {
            _logger.LogInformation($"Request to delete microservice by id{id}");
            await _service.DeleteMicroservice(id);
            _logger.LogInformation($"Response to a request for delete microservice by id{id}");
            return NoContent();
        }

        //api/microservices/42
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Restore microservice by id")]
        public async Task<ActionResult> RestoreMicroserviceById(int id)
        {
            _logger.LogInformation($"Request to restore microservice by id{id}");
            await _service.RestoreMicroservice(id);
            _logger.LogInformation($"Response to a request for restore microservice by id{id}");
            return NoContent();
        }

        //api/microservices
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation("Get all microservices")]
        public async Task<ActionResult<List<MicroserviceOutputModel>>> GetAllMicroservices()
        {
            _logger.LogInformation($"Request to get all microservices");
            var services = _map.Map<List<MicroserviceOutputModel>>(await _service.GetAllMicroservices());
            _logger.LogInformation($"Response to a request for get all microservices");

            return Ok(services);
        }

        //api/microservices/42
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Update microservice by id")]
        public async Task<ActionResult> UpdateMicroserviceById(int id, [FromBody] MicroserviceInputModel model)
        {
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
        [SwaggerOperation("Get microservices with configs by id")]
        public async Task<ActionResult<MicroserviceWithConfigsOutputModel>> GetMicroserviceWithConfigsById(int id)
        {
            _logger.LogInformation($"Request to get microservice with configs by id{id}");
            var services = _map.Map<MicroserviceWithConfigsOutputModel>(await _service.GetMicroserviceWithConfigsById(id));
            _logger.LogInformation($"Response to a request for get microservice with configs by id{id}");
            return Ok(services);
        }
    }
}
