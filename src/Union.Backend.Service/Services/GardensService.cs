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
            var gardens = db.Gardens.Include(g => g.Photos)
                                    .Include(g => g.Criteria)
                                    .Include(g => g.Location)
                                    .Include(g => g.Owner)
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
            var queryable = options.ApplyTo(db.Gardens.Where(g => g.Validation.Equals(Status.Accepted)
                                                                  && !g.IsReserved))
                                   .OfType<Garden>();

            var gardens = queryable.Include(g => g.Photos)
                                   .Include(g => g.Criteria)
                                   .Include(g => g.Location)
                                   .Include(g => g.Owner)
                                   .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task<QueryResults<List<GardenDto>>> GetGardensByUserId(Guid myId)
        {
            var gardens = db.Gardens.Include(g => g.Photos)
                                    .Include(g => g.Criteria)
                                    .Include(g => g.Owner)
                                    .Where(g => g.Owner.Id.Equals(myId))
                                    .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task<QueryResults<GardenDto>> GetGardenById(Guid gardenId)
        {
            var garden = await db.Gardens.Include(g => g.Photos)
                                         .Include(g => g.Criteria)
                                         .Include(g => g.Owner)
                                         .GetByIdAsync(gardenId) ?? throw new NotFoundApiException();

            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> AddGarden(Guid me, GardenDto dto)
        {
            var owner = await db.Users.GetByIdAsync(me) ?? throw new NotFoundApiException();

            dto.IsReserved = false;
            dto.Validation = Status.Pending;
            var garden = dto.ConvertToModel();

            owner.Gardens = owner.Gardens ?? new List<Garden>();
            owner.Gardens.Add(garden);

            await db.SaveChangesAsync();

            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> ChangeGarden(Guid me, Guid gardenId, GardenDto dto)
        {
            var garden = db.Gardens.Include(g => g.Photos)
                                   .Include(g => g.Location)
                                   .Include(g => g.Criteria)
                                   .Include(g => g.Owner)
                                   .GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();
            if (!garden.Owner.Equals(me))
                throw new ForbiddenApiException();

            garden.Name = dto.Name ?? garden.Name;
            garden.MinUse = dto.MinUse ?? garden.MinUse;
            garden.Description = dto.Description ?? garden.Description;
            garden.Location = dto.Location?.ConvertToModel() ?? garden.Location;
            garden.Criteria = dto.Criteria?.ConvertToModel() ?? garden.Criteria;
            garden.Photos = dto.Photos?.ConvertToModel<Garden>() ?? garden.Photos;

            db.Gardens.Update(garden);
            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> ChangeGardenValidation(Guid gardenId, ValidationDto val)
        {
            var garden = await db.Gardens.Include(g => g.Photos)
                                         .Include(g => g.Location)
                                         .Include(g => g.Criteria)
                                         .Include(g => g.Owner)
                                         .GetByIdAsync(gardenId) ?? throw new NotFoundApiException();
            if (!garden.Validation.Equals(Status.Pending))
                throw new UnauthorizeApiException();

            garden.Validation = val.Status;

            db.Gardens.Update(garden);

            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task ReportGarden(Guid gardenId)
        {
            var garden = await db.Gardens.GetByIdAsync(gardenId) ?? throw new NotFoundApiException();
            garden.IsReported = true;

            db.Gardens.Update(garden);
            await db.SaveChangesAsync();
        }

        public async Task<QueryResults<List<GardenDto>>> GetReportedGardens()
        {
            var gardens = db.Gardens.Include(g => g.Photos)
                                    .Include(g => g.Criteria)
                                    .Include(g => g.Owner)
                                    .Where(g => g.IsReported)
                                    .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task DeleteGarden(Guid gardenId)
        {
            var garden = db.Gardens.GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();

            db.Gardens.Remove(garden);

            await db.SaveChangesAsync();
        }
    }
}
