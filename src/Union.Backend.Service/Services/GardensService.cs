using Microsoft.AspNet.OData.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<QueryResults<List<GardenDto>>> GetPendingGardens()
        {
            var gardens = db.Gardens
                .Where(g => g.Validation.Equals(Status.Pending))
                .Select(g => g.ConvertToDto());
            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task<QueryResults<List<GardenDto>>> SearchGardens(ODataQueryOptions<Garden> options)
        {
            var queryable = options.ApplyTo(db.Gardens.Where(g => g.Validation.Equals(Status.Accepted)))
                                   .OfType<Garden>();

            var gardens = queryable.Include(g => g.Photos)
                                   .Include(g => g.Tenant)
                                   .Include(g => g.Criteria)
                                   .Include(g => g.Location)
                                   .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task<QueryResults<List<GardenDto>>> GetMyGardens(Guid myId)
        {
            var gardens = db.Score
                .Include(g => g.Photos)
                .Include(g => g.Tenant)
                .Include(g => g.Validation)
                .Include(g => g.Criteria)
                .Where(g => g.Owner.Equals(myId))
                .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task<QueryResults<GardenDto>> GetGardenById(Guid gardenId)
        {
            var garden = await db.Score
                .Include(g => g.Photos)
                .Include(g => g.Tenant)
                .Include(g => g.Validation)
                .Include(g => g.Criteria)
                .GetByIdAsync(gardenId) ?? throw new NotFoundApiException();

            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> AddGarden(GardenDto gardenDto)
        {
            gardenDto.IsReserved = false;

            var createdGarden = gardenDto.ConvertToModel();
            await db.Score.AddAsync(createdGarden);

            createdGarden.Validation = Status.Pending;

            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = createdGarden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> ChangeGarden(Guid me, Guid gardenId, GardenDto dto)
        {
            var garden = db.Gardens
                .Include(g => g.Location)
                .Include(g => g.Criteria)
                .GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();
            if (!garden.Owner.Equals(me))
                throw new ForbidenApiException();

            garden.Name = dto.Name ?? garden.Name;
            garden.Size = dto.Size ?? garden.Size;
            garden.MinUse = dto.MinUse ?? garden.MinUse;
            garden.Description = dto.Description ?? garden.Description;
            garden.Location = dto.Location?.ConvertToModel() ?? garden.Location;
            garden.Criteria = dto.Criteria?.ConvertToModel(gardenId) ?? garden.Criteria;

            db.Gardens.Update(garden);
            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> ChangeGardenValidation(Guid gardenId, ValidationDto val)
        {
            var foundGarden = db.Gardens.GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();
            if (!foundGarden.Validation.Equals(Status.Pending))
                throw new UnauthorizeApiException();

            foundGarden.Validation = val.Status;

            db.Score.Update(foundGarden);
            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = foundGarden.ConvertToDto()
            };
        }

        public async Task DeleteGarden(Guid me, Guid gardenId)
        {
            var foundGarden = db.Gardens.GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();
            if (!foundGarden.Owner.Equals(me))
                throw new ForbidenApiException();

            db.Gardens.Remove(foundGarden);
            await db.SaveChangesAsync();
        }
    }
}
