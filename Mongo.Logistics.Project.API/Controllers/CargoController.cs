using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mongo.Logistics.Project.API.Dtos;
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

        private readonly ILogger<CargoController> _logger;

        public CargoController(ILogger<CargoController> logger)
        {
            _logger = logger;
        }

        [HttpPost("{location}/to/{destination}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<bool> AddNewCargoLocation(string location, string destination)
        {
            return true;

            //error if neither  location nor destination exist as cities.Set status to "in progress"
            //TODO: return 404 if null
            //. Error if city does not exist.
            //Thing thingFromDB = await GetThingFromDBAsync();
            //if (thingFromDB == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound); // This returns HTTP 404
            //}
            //// Process thingFromDB, blah blah blah
            //return thing;
        }

        [HttpPut("{id}/delivered")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<bool> UpdateCargoToDelivered(string id)
        {
            return true;


            //TODO: return 404 if null
            //Thing thingFromDB = await GetThingFromDBAsync();
            //if (thingFromDB == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound); // This returns HTTP 404
            //}
            //// Process thingFromDB, blah blah blah
            //return thing;
        }


        [HttpPut("{id}/courier/{courier}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<bool> UpdateCourierOnloaded(string id, string courier)
        {
            return true;


            //TODO: return 404 if null
            //Thing thingFromDB = await GetThingFromDBAsync();
            //if (thingFromDB == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound); // This returns HTTP 404
            //}
            //// Process thingFromDB, blah blah blah
            //return thing;
        }

        [HttpDelete("{id}/courier")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<bool> RemoveCourier(string id)
        {
            return true;


            //TODO: return 404 if null
            //. Error if city does not exist.
            //Thing thingFromDB = await GetThingFromDBAsync();
            //if (thingFromDB == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound); // This returns HTTP 404
            //}
            //// Process thingFromDB, blah blah blah
            //return thing;
        }

        [HttpPut("{id}/location/{location}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<bool> UpdateLocation(string id, string location)
        {
            return true;


            //TODO: return 404 if null
            //. Error if city does not exist.
            //Thing thingFromDB = await GetThingFromDBAsync();
            //if (thingFromDB == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound); // This returns HTTP 404
            //}
            //// Process thingFromDB, blah blah blah
            //return thing;
        }

        [HttpGet("location/{location}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaneDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<PlaneDto> GetByLocation(string location)
        {
            return new PlaneDto
            {
                Callsign = "CARGO14",
                CurrentLocation = new double[] { 10.1797, 36.8028 },
                Heading = 0,
                Landed = "",
            };

            //TODO: return 404 if null
            //Thing thingFromDB = await GetThingFromDBAsync();
            //if (thingFromDB == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound); // This returns HTTP 404
            //}
            //// Process thingFromDB, blah blah blah
            //return thing;
        }
    }
}
