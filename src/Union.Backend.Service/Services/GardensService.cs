using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
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

        public async Task<QueryResults<List<GardenDto>>> GetAllGardens()
        {
            throw new WorkInProgressApiException();
        }
        public async Task<QueryResults<List<GardenDto>>> GetGardenByParams()
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<List<GardenDto>>> GetGardenById(Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<List<GardenDto>>> GetGardensByUser(Guid UserId)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<GardenDto>> AddGarden(GardenDto Garden)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<GardenDto>> ChangeGarden(GardenDto Garden, Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }
        public async Task<QueryResults<GardenDto>> ChangeGardenDescription(DescriptionDto desc, Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }
        public async Task<QueryResults<GardenDto>> ChangeGardenValidation(ValidationDto val, Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }

        public async Task DeleteGarden(Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }
    }
}
