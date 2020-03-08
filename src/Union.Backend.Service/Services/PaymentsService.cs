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
    public class PaymentsService
    {
        private readonly GardenLinkContext db;

        public PaymentsService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<QueryResults<PaymentDto>> GetPayment(Guid paymentId)
        {
            var pay = await db.Payments.Include(p => p.OfLeasing)
                                       .GetByIdAsync(paymentId) ?? throw new NotFoundApiException();

            return new QueryResults<PaymentDto>
            {
                Data = pay.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<PaymentDto>>> GetAllPayments()
        {
            var pays = db.Payments.Include(p => p.Leasing)
                                  .Select(u => u.ConvertToDto());

            return new QueryResults<List<PaymentDto>>
            {
                Data = await pays.ToListAsync(),
                Count = await pays.CountAsync()
            };
        }

        public async Task<QueryResults<List<PaymentDto>>> GetMyPayments(Guid myId)
        {
            var pays = db.Payments.Include(p => p.Leasing)
                                  .Where(p => p.Leasing.Garden.Owner.Id.Equals(myId) || p.Leasing.Renter.Id.Equals(myId))
                                  .Select(p => p.ConvertToDto());

            return new QueryResults<List<PaymentDto>>
            {
                Data = await pays.ToListAsync(),
                Count = await pays.CountAsync()
            };
        }

        public async Task<QueryResults<PaymentDto>> AddPayment(PaymentDto dto)
        {
            var leasing = await db.Leasings.GetByIdAsync(dto.Leasing) ?? throw new NotFoundApiException();
            if (leasing.State.Equals(LeasingStatus.Refused) || leasing.State.Equals(LeasingStatus.Finished))
                throw new BadRequestApiException("It is no longer possible to pay for this contract.");

            dto.Date = DateTime.UtcNow;
            leasing.Payment = dto.ConvertToModel();

            await db.SaveChangesAsync();

            return new QueryResults<PaymentDto>
            {
                Data = leasing.Payment.ConvertToDto()
            };
        }

        public async Task<QueryResults<PaymentDto>> ChangePayment(Guid id, PaymentDto dto)
        {
            _ = await db.Leasings.GetByIdAsync(dto.Leasing) ?? throw new NotFoundApiException();
            var pay = db.Payments.GetByIdAsync(id).Result ?? throw new NotFoundApiException();

            pay.Sum = dto.Sum;

            db.Payments.Update(pay);
            await db.SaveChangesAsync();

            return new QueryResults<PaymentDto>
            {
                Data = pay.ConvertToDto()
            };
        }

        public async Task DeletePayment(Guid paymentId)
        {
            var pay = await db.Payments.GetByIdAsync(paymentId) ?? throw new NotFoundApiException();

            db.Payments.Remove(pay);

            await db.SaveChangesAsync();
        }
    }
}
