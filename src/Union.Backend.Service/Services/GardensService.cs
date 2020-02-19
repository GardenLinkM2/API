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
        public async Task<QueryResults<List<GardenDto>>> GetGardenByParams(Criteria crit)
        {
            var gardens = db.Gardens
                .Include(g => g.Photos)
                .Include(g => g.Tenant)
                .Include(g => g.Validation)
                .Include(g => g.Criteria)
                .Where(g => g.Criteria.Area.Equals(crit.Area)
                    && g.Criteria.DirectAccess.Equals(crit.DirectAccess)
                    && g.Criteria.Equipments.Equals(crit.Equipments)
                    && g.Criteria.Location.Equals(crit.Location)
                    && g.Criteria.LocationTime.Equals(crit.LocationTime)
                    && g.Criteria.Orientation.Equals(crit.Orientation)
                    && g.Criteria.Price.Equals(crit.Price)
                    && g.Criteria.TypeOfClay.Equals(crit.TypeOfClay)
                    && g.Criteria.WaterAccess.Equals(crit.WaterAccess)
                )
                .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task<QueryResults<GardenDto>> GetGardenById(Guid gardenId)
        {
            var garden = await db.Gardens
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

        public async Task<QueryResults<List<GardenDto>>> GetGardensByUser(Guid ownerId)
        {
            var gardens = db.Gardens
                .Include(g => g.Photos)
                .Include(g => g.Tenant)
                .Include(g => g.Validation)
                .Include(g => g.Criteria)
                .Where(g => g.IdOwner.Equals(ownerId))
                .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task<QueryResults<GardenDto>> AddGarden(GardenDto gardenDto, Guid id)
        {
            gardenDto.Id = id;

            var createdGarden = gardenDto.ConvertToModel();
            createdGarden.Validation = new Validation
            {
                ForGarden = id,
                State = 0
            };

            await db.Gardens.AddAsync(createdGarden);
            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = createdGarden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> ChangeGarden(GardenDto garden, Guid gardenId)
        {
            var foundGarden = db.Gardens.GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();

            foundGarden.Name = garden.Name;
            foundGarden.Size = garden.Size;
            foundGarden.Reserve = garden.Reserve;
            foundGarden.MinUse = garden.MinUse;
            foundGarden.Validation = garden.Validation.ConvertToModel(gardenId);
            foundGarden.Criteria = garden.Criteria.ConvertToModel(gardenId);
            foundGarden.Photos = garden.Photos.ConvertToModel<Garden>(gardenId);

            db.Gardens.Update(foundGarden);
            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = new GardenDto { Id = foundGarden.Id }
            };
        }
        public async Task<QueryResults<GardenDto>> ChangeGardenDescription(DescriptionDto desc, Guid GardenId)
        {
            throw new WorkInProgressApiException();
        }
        public async Task<QueryResults<GardenDto>> ChangeGardenValidation(ValidationDto val, Guid gardenId)
        {
            var foundGarden = db.Gardens.GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();

            Validation toChange = val.ConvertToModel(gardenId);
            foundGarden.Validation = toChange;

            db.Gardens.Update(foundGarden);
            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = foundGarden.ConvertToDto()
            };
        }

        public async Task DeleteGarden(Guid gardenId)
        {
            var foundGarden = db.Gardens.GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();
            db.Gardens.Remove(foundGarden);
            await db.SaveChangesAsync();
        }
    }
}
