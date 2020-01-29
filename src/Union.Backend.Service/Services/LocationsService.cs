using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Model.Models;
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


        public async Task<LocationQueryResults> GetLocation(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<LocationsQueryResults> GetAllLocations()
        {
            throw new NotImplementedException();
        }

        public async Task<LocationQueryResults> AddLocation(LocationDto Locationd)
        {
            throw new NotImplementedException();
        }

        public async Task<LocationQueryResults> ChangeLocation(LocationDto Location, Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteLocation(Guid LocationId)
        {
            throw new NotImplementedException();
        }
    }
}
