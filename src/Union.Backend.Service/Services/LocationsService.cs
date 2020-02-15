using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Results;

namespace Union.Backend.Service.Services
{
    public class LocationsService
    {
        private readonly GardenLinkContext db;
        public LocationsService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }


        public async Task<QueryResults<LocationDto>> GetLocation(Guid Id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<List<LocationDto>>> GetAllLocations()
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<LocationDto>> AddLocation(LeasingDto Locationd)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<LocationDto>> ChangeLocation(LeasingDto Location, Guid id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task DeleteLocation(Guid LocationId)
        {
            throw new WorkInProgressApiException();
        }
    }
}
