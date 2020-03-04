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
        private readonly UsersService uService;
        public LeasingsService(GardenLinkContext gardenLinkContext, UsersService u)
        {
            db = gardenLinkContext;
            uService = u;
        }


        public async Task<QueryResults<LeasingDto>> GetLeasing(Guid leasingId)
        {
            var leasing = await db.Leasings
                .Include(l => l.Garden)
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
            var leasings = db.Leasings
                .Include(l => l.Garden)
                .Include(l => l.Owner)
                .Include(l => l.Renter)
                .Select(l => l.ConvertToDto());

            return new QueryResults<List<LeasingDto>>
            {
                Data = await leasings.ToListAsync(),
                Count = await leasings.CountAsync()
            };
        }

        public async Task<QueryResults<LeasingDto>> AddLeasing(LeasingDto leasing)
        {
            var renter = (await uService.GetUserById(leasing.Renter)).Data.ConvertToModel();
            var owner = (await uService.GetUserById(leasing.Owner)).Data.ConvertToModel();

            var createdLeasing = leasing.ConvertToModel(renter, owner);
            await db.Leasings.AddAsync(createdLeasing);

            await db.SaveChangesAsync();
            return new QueryResults<LeasingDto>
            {
                Data = createdLeasing.ConvertToDto()
            };
        }

        public async Task<QueryResults<LeasingDto>> ChangeLeasing(LeasingDto Leasing, Guid id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task DeleteLeasing(Guid LeasingId)
        {
            throw new WorkInProgressApiException();
        }
    }
}
