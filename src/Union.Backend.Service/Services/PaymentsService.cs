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
    public class PaymentsService
    {
        private readonly GardenLinkContext db;

        public PaymentsService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<QueryResults<List<PaymentDto>>> GetAllPayments()
        {
            var pays = db.Payments.Select(u => u.ConvertToDto());

            return new QueryResults<List<PaymentDto>>
            {
                Data = await pays.ToListAsync(),
                Count = await pays.CountAsync()
            };
        }

        public async Task<QueryResults<PaymentDto>> GetPayment(Guid paymentId)
        {
            var pay = await db.Payments.GetByIdAsync(paymentId) ?? throw new NotFoundApiException();

            return new QueryResults<PaymentDto>
            {
                Data = pay.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<PaymentDto>>> GetMyPayments(Guid myId)
        {
            var pays = db.Payments.Include(p => p.Leasing)
                                  .Where(p => p.Leasing.Owner.Equals(myId) || p.Leasing.Renter.Equals(myId))
                                  .Select(p => p.ConvertToDto());

            return new QueryResults<List<PaymentDto>>
            {
                Data = await pays.ToListAsync(),
                Count = await pays.CountAsync()
            };
        }

        public async Task<QueryResults<PaymentDto>> AddPayment(PaymentDto dto)
        {
            var leasing = await db.Leasings.Include(l => l.Renter)
                                           .Include(l => l.Garden)
                                           .GetByIdAsync(dto.Leasing) ?? throw new NotFoundApiException();

            var pay = dto.ConvertToModel(leasing);

            await db.Payments.AddAsync(pay);
            await db.SaveChangesAsync();

            return new QueryResults<PaymentDto>
            {
                Data = pay.ConvertToDto()
            };
        }

        public async Task<QueryResults<PaymentDto>> ChangePayment(Guid id, PaymentDto dto)
        {
            var pay = db.Payments.GetByIdAsync(id).Result ?? throw new NotFoundApiException();

            pay.Sum = dto.Sum;
            pay.State = dto.State;
            pay.Leasing = await db.Leasings.GetByIdAsync(dto.Leasing) ?? throw new NotFoundApiException();

            db.Payments.Update(pay);
            await db.SaveChangesAsync();

            return new QueryResults<PaymentDto>
            {
                Data = new PaymentDto { Id = pay.Id }
            };
        }

        public async Task DeletePayment(Guid PaymentId)
        {
            var foundPay = db.Payments.GetByIdAsync(PaymentId).Result ?? throw new NotFoundApiException();
            db.Payments.Remove(foundPay);
            await db.SaveChangesAsync();
        }
    }
}
