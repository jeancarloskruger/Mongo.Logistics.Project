using AutoMapper;
using Mongo.Logistics.Project.API.DAL;
using Mongo.Logistics.Project.API.Dtos;
using Mongo.Logistics.Project.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.Services
{
    public class PlaneService
    {
        private readonly PlaneRepository planeRepository;
        private readonly CityRepository cityRepository;
        private readonly IMapper mapper;

        public PlaneService(IMapper mapper, PlaneRepository planeRepository, CityRepository cityRepository)
        {
            this.mapper = mapper;
            this.planeRepository = planeRepository;
            this.cityRepository = cityRepository;
        }

        public async Task<List<PlaneDto>> GetAllAsync()
        {
            return mapper.Map<List<PlaneDto>>(await planeRepository.GetAllAsync());
        }

        public async Task<PlaneDto> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            return mapper.Map<PlaneDto>(await planeRepository.GetByIdAsync(id));
        }

        public async Task<PlaneDto> UpdateLocationAndHeading(string id, string location, int heading)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var plane = await planeRepository.GetByIdAsync(id);
            if (plane == null)
            {
                return null;
            }

            plane.CurrentLocation = location.GetLocation();
            plane.Heading = heading;

            var update = Builders<Plane>.Update
                 .Set(s => s.Heading, plane.Heading)
                 .Set(s => s.CurrentLocation, plane.CurrentLocation);

            return mapper.Map<PlaneDto>(await planeRepository.UpdadeOneAsync(plane.Id, update));
        }

        public async Task<PlaneDto> UpdateLocationAndHeadingAndCity(string id, string location, int heading, string city)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(city))
            {
                return null;
            }
            var cityExist = await cityRepository.GetByIdAsync(city);

            if (cityExist == null)
            {
                return null;
            }

            var plane = await planeRepository.GetByIdAsync(id);

            if (plane == null)
            {
                return null;
            }

            plane.CurrentLocation = location.GetLocation();
            plane.Heading = heading;
            plane.Landed = city;

            var update = Builders<Plane>.Update
                .Set(s => s.Heading, plane.Heading)
                .Set(s => s.CurrentLocation, plane.CurrentLocation)
                .Set(s => s.Landed, plane.Landed);

            return mapper.Map<PlaneDto>(await planeRepository.UpdadeOneAsync(plane.Id, update));

        }

        public async Task<PlaneDto> AddOrUpdatePlaneRoute(string id, string city, bool overrideAllList)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(city))
            {
                return null;
            }
            var cityExist = await cityRepository.GetByIdAsync(city);

            if (cityExist == null)
            {
                return null;
            }

            var plane = await planeRepository.GetByIdAsync(id);

            if (plane == null)
            {
                return null;
            }

            UpdateDefinition<Plane> update;
            if (overrideAllList)
            {
                update = Builders<Plane>.Update
                .Set(s => s.Route, new string[] { city });
            }
            else
            {
                update = Builders<Plane>.Update
                    .AddToSet(s => s.Route, city);
            }

            return mapper.Map<PlaneDto>(await planeRepository.UpdadeOneAsync(plane.Id, update));

        }

        public async Task<PlaneDto> ReachedNextDestination(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var plane = await planeRepository.GetByIdAsync(id);
            if (plane == null)
            {
                return null;
            }

            var update = Builders<Plane>.Update
               .PopFirst(s => s.Route);

            return mapper.Map<PlaneDto>(await planeRepository.UpdadeOneAsync(plane.Id, update));
        }
    }
}
