using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mongo.Logistics.Project.API.Dtos;
using Mongo.Logistics.Project.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.Controllers
{
    [ApiController]
    [Route("cargo")]
    public class CargoController : ControllerBase
    {
        private readonly CargoService cargoService;

        public CargoController(CargoService cargoService)
        {
            this.cargoService = cargoService;
        }

        [HttpPost("{location}/to/{destination}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CargoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewCargo(string location, string destination)
        {
            var objDtos = await cargoService.AddNewCargo(location, destination);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpPut("{id}/delivered")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CargoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCargoToDelivered(string id)
        {
            var objDtos = await cargoService.UpdateCargoToDelivered(id);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }


        [HttpPut("{id}/courier/{courier}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CargoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCourierOnloaded(string id, string courier)
        {
            var objDtos = await cargoService.UpdateCourierOnloaded(id, courier);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpDelete("{id}/courier")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CargoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveCourier(string id)
        {
            var objDtos = await cargoService.RemoveCourier(id);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpPut("{id}/location/{location}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CargoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLocation(string id, string location)
        {
            var objDtos = await cargoService.UpdateLocation(id, location);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }

        [HttpGet("location/{location}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CargoDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByLocation(string location)
        {
            var objDtos = await cargoService.GetAllInProgressByLocation(location);
            if (objDtos == null)
            {
                return NotFound();
            }
            return Ok(objDtos);
        }
    }
}