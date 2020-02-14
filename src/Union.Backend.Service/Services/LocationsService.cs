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
            throw new WorkInProgressApiException();
        }

        public async Task<LocationsQueryResults> GetAllLocations()
        {
            throw new WorkInProgressApiException();
        }

        public async Task<LocationQueryResults> AddLocation(LeasingDto Locationd)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<LocationQueryResults> ChangeLocation(LeasingDto Location, Guid id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task DeleteLocation(Guid LocationId)
        {
            throw new WorkInProgressApiException();
        }
    }
}
