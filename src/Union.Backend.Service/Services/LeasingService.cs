using System;
using System.Collections.Generic;
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
        public LeasingsService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }


        public async Task<QueryResults<LeasingDto>> GetLeasing(Guid Id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<List<LeasingDto>>> GetAllLeasings()
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<LeasingDto>> AddLeasing(LeasingDto Leasingd)
        {
            throw new WorkInProgressApiException();
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
