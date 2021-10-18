using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mongo.Logistics.Project.API.Dtos;
using Mongo.Logistics.Project.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.Controllers
{
    [ApiController]
    [Route("planes")]
    public class PlaneController : ControllerBase
    {

        private readonly PlaneService planeService;

        public PlaneController(PlaneService planeService)
        {
            this.planeService = planeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlaneDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var objDtos = await planeService.GetAllAsync();
            if (objDtos == null || objDtos.Count == 0)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaneDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
            var objDtos = await planeService.GetByIdAsync(id);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpPut("{id}/location/{location}/{heading}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaneDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLocationAndHeading(string id, string location, int heading)
        {
            var objDtos = await planeService.UpdateLocationAndHeading(id, location, heading);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpPut("{id}/location/{location}/{heading}/{city}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaneDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLocationAndHeadingAndCity(string id, string location, int heading, string city)
        {
            var objDtos = await planeService.UpdateLocationAndHeadingAndCity(id, location, heading, city);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpPut("{id}/route/{city}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaneDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePlaneRoute(string id, string city)
        {
            var objDtos = await planeService.AddOrUpdatePlaneRoute(id, city, true);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpPost("{id}/route/{city}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaneDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPlaneRoute(string id, string city)
        {
            var objDtos = await planeService.AddOrUpdatePlaneRoute(id, city, false);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpDelete("{id}/route/destination")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaneDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReachedNextDestination(string id)
        {
            var objDtos = await planeService.ReachedNextDestination(id);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }
    }
}
