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
    public class GardensService
    {
        private readonly GardenLinkContext db;
        public GardensService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<GardensQueryResults> GetAllGardens()
        {
            throw new WorkInProgressApiException();
        }
        public async Task<GardensQueryResults> GetGardenByParams()
        {
            throw new WorkInProgressApiException();
        }

        public async Task<GardenQueryResults> GetGardenById(Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<GardensQueryResults> GetGardensByUser(Guid UserId)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<GardenQueryResults> AddGarden(GardenDto Garden)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<GardenQueryResults> ChangeGarden(GardenDto Garden, Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }
        public async Task<GardenQueryResults> ChangeGardenDescription(DescriptionDto desc, Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }
        public async Task<GardenQueryResults> ChangeGardenValidation(ValidationDto val, Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }

        public async Task DeleteGarden(Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }
    }
}
