using AutoMapper;
using Mongo.Logistics.Project.API.Consts;
using Mongo.Logistics.Project.API.DAL;
using Mongo.Logistics.Project.API.Dtos;
using Mongo.Logistics.Project.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.Services
{
    public class CargoService
    {
        private readonly CityRepository cityRepository;
        private readonly CargoRepository cargoRepository;
        private readonly PlaneRepository planeRepository;
        private readonly IMapper mapper;

        public CargoService(CityRepository cityRepository, IMapper mapper, CargoRepository cargoRepository, PlaneRepository planeRepository)
        {
            this.cityRepository = cityRepository;
            this.mapper = mapper;
            this.cargoRepository = cargoRepository;
            this.planeRepository = planeRepository;
        }

        public async Task<CargoDto> UpdateCargoToDelivered(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var cargoExist = await cargoRepository.GetByIdAsync(id);

            if (cargoExist == null)
            {
                return null;
            }

            cargoExist.Status = CargoStatusConst.Delivered;
            cargoExist.Received = DateTime.UtcNow;

            var update = Builders<Cargo>.Update
                .Set(s => s.Status, cargoExist.Status)
                .Set(s => s.Received, cargoExist.Received);

            return mapper.Map<CargoDto>(await cargoRepository.UpdadeOneAsync(cargoExist.Id, update));
        }

        public async Task<CargoDto> AddNewCargo(string locationCity, string destinationCity)
        {
            if (string.IsNullOrWhiteSpace(locationCity) || string.IsNullOrWhiteSpace(destinationCity))
            {
                return null;
            }
            var locationCityExist = await cityRepository.GetByIdAsync(locationCity);

            if (locationCityExist == null)
            {
                return null;
            }

            var destinationCityExist = await cityRepository.GetByIdAsync(destinationCity);

            if (destinationCityExist == null)
            {
                return null;
            }
            var cargo = new Cargo
            {
                Location = locationCity,
                LocationType = CargoLocationTypeConst.City,
                Destination = destinationCity,
                Status = CargoStatusConst.InProgress
            };

            await cargoRepository.InsertOneAsync(cargo);

            return mapper.Map<CargoDto>(cargo);
        }

        public async Task<CargoDto> UpdateCourierOnloaded(string id, string courier)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var cargoExist = await cargoRepository.GetByIdAsync(id);

            if (cargoExist == null)
            {
                return null;
            }

            var planeExist = await planeRepository.GetByIdAsync(courier);

            if (planeExist == null)
            {
                return null;
            }

            cargoExist.Courier = courier;

            var update = Builders<Cargo>.Update
                .Set(s => s.Courier, cargoExist.Courier);

            return mapper.Map<CargoDto>(await cargoRepository.UpdadeOneAsync(cargoExist.Id, update));
        }

        public async Task<CargoDto> UpdateLocation(string id, string location)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var cargoExist = await cargoRepository.GetByIdAsync(id);

            if (cargoExist == null)
            {
                return null;
            }

            if (cargoExist.LocationType == CargoLocationTypeConst.City)
            {
                var planeExist = await planeRepository.GetByIdAsync(location);

                if (planeExist == null)
                {
                    return null;
                }

                cargoExist.Location = location;
                cargoExist.LocationType = CargoLocationTypeConst.Plane;

            }
            else
            {
                var cityExist = await cityRepository.GetByIdAsync(location);

                if (cityExist == null)
                {
                    return null;
                }

                cargoExist.Location = location;
                cargoExist.LocationType = CargoLocationTypeConst.City;
            }

            var update = Builders<Cargo>.Update
                 .Set(s => s.Location, cargoExist.Location)
                 .Set(s => s.LocationType, cargoExist.LocationType);

            return mapper.Map<CargoDto>(await cargoRepository.UpdadeOneAsync(cargoExist.Id, update));
        }

        public async Task<List<CargoDto>> GetAllInProgressByLocation(string location)
        {
            return mapper.Map<List<CargoDto>>(await cargoRepository.GetAllInProgressByLocation(location));
        }

        public async Task<CargoDto> RemoveCourier(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var cargoExist = await cargoRepository.GetByIdAsync(id);

            if (cargoExist == null)
            {
                return null;
            }

            cargoExist.Courier = null;

            var update = Builders<Cargo>.Update
                .Set(s => s.Courier, cargoExist.Courier);

            return mapper.Map<CargoDto>(await cargoRepository.UpdadeOneAsync(cargoExist.Id, update));
        }
    }
}
