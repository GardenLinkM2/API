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
    public class LeasingsService
    {
        private readonly GardenLinkContext db;
        public LeasingsService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<QueryResults<LeasingDto>> GetLeasing(Guid leasingId)
        {
            var leasing = await db.Leasings.Include(l => l.Garden)
                                           .Include(l => l.Garden.Owner)
                                           .Include(l => l.Renter)
                                           .GetByIdAsync(leasingId) ?? throw new NotFoundApiException();

            return new QueryResults<LeasingDto>
            {
                Data = leasing.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<LeasingDto>>> GetAllLeasings()
        {
            var leasings = db.Leasings.Include(l => l.Garden)
                                      .Include(l => l.Garden.Owner)
                                      .Include(l => l.Renter)
                                      .Select(l => l.ConvertToDto());

            return new QueryResults<List<LeasingDto>>
            {
                Data = await leasings.ToListAsync(),
                Count = await leasings.CountAsync()
            };
        }

        public async Task<QueryResults<List<LeasingDto>>> GetAllLeasingsByUserId(Guid userId)
        {
            var leasings = db.Leasings.Include(l => l.Garden)
                                      .Include(l => l.Garden.Owner)
                                      .Include(l => l.Renter)
                                      .Where(l => l.Garden.Owner.Id.Equals(userId) || l.Renter.Id.Equals(userId))
                                      .Select(l => l.ConvertToDto());

            return new QueryResults<List<LeasingDto>>
            {
                Data = await leasings.ToListAsync(),
                Count = await leasings.CountAsync()
            };
        }

        public async Task<QueryResults<LeasingDto>> AddLeasing(Guid me, LeasingDto dto)
        {
            var renter = await db.Users.Include(u => u.Wallet)
                                       .ThenInclude(w => w.OfUser)
                                           .ThenInclude(u => u.AsRenter)
                                               .ThenInclude(l => l.Garden)
                                                   .ThenInclude(g => g.Criteria)
                                       .GetByIdAsync(me) ?? throw new NotFoundApiException();
            var garden = await db.Gardens.Include(g => g.Owner)
                                         .Include(g => g.Criteria)
                                         .GetByIdAsync(dto.Garden) ?? throw new NotFoundApiException();

            if (!garden.Validation.Equals(Status.Accepted))
                throw new BadRequestApiException("The garden must be validated to be able to rent it.");

            if (garden.Owner.Id.Equals(me))
                throw new BadRequestApiException("You are not authorize to rent your own garden.");

            if (dto.End <= dto.Begin)
                throw new BadRequestApiException("Your end date is less than your start date.");

            dto.Creation = DateTime.UtcNow.ToTimestamp();
            dto.State = LeasingStatus.InDemand;
            dto.Renew = false;
            dto.Renter = renter.Id;

            var leasing = dto.ConvertToModel();

            var months = Utils.MonthDifference(leasing.End, leasing.Begin);
            if (renter.Wallet.RealTimeBalance - (garden.Criteria.Price * months) < 0)
                throw new BadRequestApiException("You have not enough money to rent this garden.");

            renter.AsRenter = renter.AsRenter ?? new List<Leasing>();
            renter.AsRenter.Add(leasing);

            garden.Leasings = garden.Leasings ?? new List<Leasing>();
            garden.Leasings.Add(leasing);

            await db.SaveChangesAsync();

            return new QueryResults<LeasingDto>
            {
                Data = leasing.ConvertToDto()
            };
        }

        public async Task<QueryResults<LeasingDto>> ChangeLeasing(Guid me, Guid leasingId, LeasingDto dto, bool isAdmin)
        {
            var leasing = db.Leasings.Include(l => l.Garden)
                                     .Include(l => l.Renter)
                                     .Include(l => l.Garden.Owner)
                                     .GetByIdAsync(leasingId).Result ?? throw new NotFoundApiException();

            if(!leasing.Renter.Id.Equals(me) && !leasing.Garden.Owner.Id.Equals(me) && !isAdmin)
                throw new ForbiddenApiException();

            leasing.Begin = dto.Begin?.ToDateTime() ?? leasing.Begin;
            leasing.End = dto.End?.ToDateTime() ?? leasing.End;
            leasing.Renew = dto.Renew ?? leasing.Renew;
            leasing.State = dto.State ?? leasing.State;

            db.Update(leasing);
            await db.SaveChangesAsync();

            return new QueryResults<LeasingDto>
            {
                Data = leasing.ConvertToDto()
            };
        }

        public async Task DeleteLeasing(Guid leasingId)
        {
            var leasing = await db.Leasings.Include(l => l.Payment)
                                           .GetByIdAsync(leasingId) ?? throw new NotFoundApiException();

            db.Leasings.Remove(leasing);
            if(leasing.Payment != null)
                db.Payments.Remove(leasing.Payment);

            await db.SaveChangesAsync();
        }
    }
}
