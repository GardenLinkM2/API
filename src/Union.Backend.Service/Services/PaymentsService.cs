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
        private readonly LeasingsService leasingService;
        public PaymentsService(GardenLinkContext gardenLinkContext, LeasingsService leasingService)
        {
            this.leasingService = leasingService;
            db = gardenLinkContext;
        }


        public async Task<QueryResults<PaymentDto>> GetPayment(Guid paymentId)
        {
            var pay = await db.Payments
                .GetByIdAsync(paymentId) ?? throw new NotFoundApiException();

            return new QueryResults<PaymentDto>
            {
                Data = pay.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<PaymentDto>>> GetAllPayments(Guid userId)
        {
            var pay = db.Payments
                .Where(u => leasingService.GetLeasing(u.Leasing).Result.Data.Owner == userId || leasingService.GetLeasing(u.Leasing).Result.Data.Renter == userId)
                .Select(u => u.ConvertToDto());

            return new QueryResults<List<PaymentDto>>
            {
                Data = await pay.ToListAsync(),
                Count = await pay.CountAsync()
            };
        }

        public async Task<QueryResults<PaymentDto>> AddPayment(PaymentDto paymentDto)
        {

            var createdPay = paymentDto.ConvertToModel();
            createdPay.Id = new Guid();

            await db.Payments.AddAsync(createdPay);
            await db.SaveChangesAsync();
            return new QueryResults<PaymentDto>
            {
                Data = createdPay.ConvertToDto()
            };
        }

        public async Task<QueryResults<PaymentDto>> ChangePayment(PaymentDto Payment, Guid id)
        {

            var foundPayment = db.Payments.GetByIdAsync(id).Result ?? throw new NotFoundApiException();

            foundPayment.Sum = Payment.Sum;
            foundPayment.State = Payment.State;
            foundPayment.Leasing = Payment.Leasing;

            db.Payments.Update(foundPayment);
            await db.SaveChangesAsync();
            return new QueryResults<PaymentDto>
            {
                Data = new PaymentDto { Id = foundPayment.Id }
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
