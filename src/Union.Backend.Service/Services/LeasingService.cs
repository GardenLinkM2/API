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
    public class LeasingsService
    {
        private readonly GardenLinkContext db;
        private readonly UsersService userService;
        public LeasingsService(GardenLinkContext gardenLinkContext, UsersService userService)
        {
            db = gardenLinkContext;
            this.userService = userService;
        }

        public async Task<QueryResults<LeasingDto>> GetLeasing(Guid leasingId)
        {
            var leasing = await db.Leasings.Include(l => l.Garden)
                                           .Include(l => l.Owner)
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
                                      .Include(l => l.Owner)
                                      .Include(l => l.Renter)
                                      .Select(l => l.ConvertToDto());

            return new QueryResults<List<LeasingDto>>
            {
                Data = await leasings.ToListAsync(),
                Count = await leasings.CountAsync()
            };
        }

        public async Task<QueryResults<LeasingDto>> AddLeasing(LeasingDto dto)
        {
            var renter = await db.Users.GetByIdAsync(dto.Renter) ?? throw new NotFoundApiException();
            var owner = await db.Users.GetByIdAsync(dto.Owner) ?? throw new NotFoundApiException();

            var leasing = dto.ConvertToModel(renter, owner);
            await db.Leasings.AddAsync(leasing);

            await db.SaveChangesAsync();
            return new QueryResults<LeasingDto>
            {
                Data = leasing.ConvertToDto()
            };
        }

        public async Task<QueryResults<LeasingDto>> ChangeLeasing(Guid id, LeasingDto leasing)
        {
            var foundLeasing = db.Leasings.GetByIdAsync(id).Result ?? throw new NotFoundApiException();

            foundLeasing.Begin = leasing.Begin;
            foundLeasing.End = leasing.End;
            foundLeasing.Price = leasing.Price;
            foundLeasing.Renew = leasing.Renew;
            foundLeasing.State = leasing.State;
            foundLeasing.Time = leasing.Time;
            foundLeasing.Garden = leasing.Garden.ConvertToModel();
            foundLeasing.Owner = (await userService.GetUserById(leasing.Owner)).Data.ConvertToModel();
            foundLeasing.Renter = (await userService.GetUserById(leasing.Renter)).Data.ConvertToModel();

            db.Update(foundLeasing);
            await db.SaveChangesAsync();
            return new QueryResults<LeasingDto>
            {
                Data = foundLeasing.ConvertToDto()
            };
        }

        public async Task DeleteLeasing(Guid leasingId)
        {
            var foundLeasing = db.Leasings.GetByIdAsync(leasingId).Result ?? throw new NotFoundApiException();
            
            db.Leasings.Remove(foundLeasing);
            await db.SaveChangesAsync();
        }
    }
}
