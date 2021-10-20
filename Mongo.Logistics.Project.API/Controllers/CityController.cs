using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mongo.Logistics.Project.API.Dtos;
using Mongo.Logistics.Project.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.Controllers
{
    [ApiController]
    [Route("cities")]
    public class CityController : ControllerBase
    {

        private readonly CityService cityService;

        public CityController(CityService cityService)
        {
            this.cityService = cityService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CityDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var objDtos = await cityService.GetAllAsync();
            if (objDtos == null || objDtos.Count == 0)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CityDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
            var objDtos = await cityService.GetByIdAsync(id);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpGet("{id}/neighbors/{count}")]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdNearbyCitiesSortedByNearestFirst(string id, int count)
        {
            var objDtos = await cityService.GetByIdNearbyCitiesSortedByNearestFirst(id, count);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

    }
}
