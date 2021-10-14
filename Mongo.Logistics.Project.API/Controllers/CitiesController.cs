using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mongo.Logistics.Project.API.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.Controllers
{
    [ApiController]
    [Route("cities")]
    public class CitiesController : ControllerBase
    {

        private readonly ILogger<CitiesController> _logger;

        public CitiesController(ILogger<CitiesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CityDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<CityDto>> Get()
        {
            return new List<CityDto> { new CityDto
            {
                Country="Lithuania",
                Location= new double[]{ 25.3166, 54.6834 },
                Name ="Vilnius"
            } };
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CityDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<CityDto> GetById(string id)
        {
            return new CityDto
            {
                Country = "United Kingdom",
                Location = new double[] { -0.1167, 51.5 },
                Name = "London"
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

        [HttpGet("{id}/neighbors/{count}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CityDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<CityDto>> GetByIdNearbyCitiesSortedByNearestFirst(string id, int count)
        {
            return new List<CityDto> { new CityDto
            {
                Country = "United Kingdom",
                Location = new double[] { -0.1167, 51.5 },
                Name = "London"
               } ,
             new CityDto
            {
                Country = "Netherlands",
                Location = new double[] { -0.1167, 51.5 },
                Name = "The Hague"
               }};

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
