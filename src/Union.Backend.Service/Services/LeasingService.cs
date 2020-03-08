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
                                      .Include(l => l.Renter)
                                      .Select(l => l.ConvertToDto());

            return new QueryResults<List<LeasingDto>>
            {
                Data = await leasings.ToListAsync(),
                Count = await leasings.CountAsync()
            };
        }

        public async Task<QueryResults<LeasingDto>> AddLeasing(Guid me, LeasingDto dto)
        {
            var renter = await db.Users.GetByIdAsync(me) ?? throw new NotFoundApiException();
            var garden = await db.Gardens.GetByIdAsync(dto.Garden) ?? throw new NotFoundApiException();

            if (!garden.Validation.Equals(Status.Accepted))
                throw new BadRequestApiException("The garden must be validated to be able to rent it.");

            dto.State = LeasingStatus.InDemand;
            dto.Renew = false;
            dto.Renter = renter.Id;

            var leasing = dto.ConvertToModel();
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

        public async Task<QueryResults<LeasingDto>> ChangeLeasing(Guid id, LeasingDto dto)
        {
            var leasing = db.Leasings.GetByIdAsync(id).Result ?? throw new NotFoundApiException();

            leasing.Begin = dto.Begin ?? leasing.Begin;
            leasing.End = dto.End ?? leasing.End;
            leasing.Renew = dto.Renew ?? leasing.Renew;
            leasing.State = dto.State ?? leasing.State;
            leasing.Time = (dto.Time?.ToTimeSpan() ?? leasing.Time);

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
            db.Payments.Remove(leasing.Payment);

            await db.SaveChangesAsync();
        }
    }
}
