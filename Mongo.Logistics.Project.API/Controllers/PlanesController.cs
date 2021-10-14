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
    [Route("planes")]
    public class PlanesController : ControllerBase
    {

        private readonly ILogger<PlanesController> _logger;

        public PlanesController(ILogger<PlanesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlaneDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<PlaneDto>> Get()
        {
            return new List<PlaneDto> { new PlaneDto
           {
                Callsign = "CARGO14",
                CurrentLocation = new double[] { 10.1797, 36.8028 },
                Heading = 0,
                Landed = "",
                Route =  new double[] { },
            } };
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaneDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<PlaneDto> GetById(string id)
        {
            return new PlaneDto
            {
                Callsign = "CARGO14",
                CurrentLocation = new double[] { 10.1797, 36.8028 },
                Heading = 0,
                Landed = "",
                Route = new double[] { },
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

        [HttpPut("{id}/location/{location}/{heading}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateUserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromForm][Required] CreateUserRequest request)
        {

            var input = new CreateUserInput(request.Name, request.Email);
            await _mediator.PublishAsync(input);
            return this._CreateUserPresenter.ViewModel;
        }


    }
}
