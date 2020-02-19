using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var gardens = db.Gardens
                .Include(g => g.Photos)
                .Include(g => g.Tenant)
                .Include(g => g.Validation)
                .Include(g => g.Criteria)
                .Select(g => g.ConvertToDto());
            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }
        public async Task<QueryResults<List<GardenDto>>> GetGardenByParams()
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<GardenDto>> GetGardenById(Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<List<GardenDto>>> GetGardensByUser(Guid OwnerId)
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
