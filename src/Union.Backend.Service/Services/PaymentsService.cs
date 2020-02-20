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
            var pay = await db.Payments
                .Include(u => u.Leasing)
                .Include(u => u.Payer)
                .Include(u => u.Collector)
                .GetByIdAsync(paymentId) ?? throw new NotFoundApiException();

            return new QueryResults<PaymentDto>
            {
                Data = pay.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<PaymentDto>>> GetAllPayments(Guid userId)
        {
            var pay = db.Payments
                .Include(u => u.Leasing)
                .Include(u => u.Payer)
                .Include(u => u.Collector)
                .Where(u => u.Payer.Id == userId || u.Collector.Id == userId)
                .Select(u => u.ConvertToDto());

            return new QueryResults<List<PaymentDto>>
            {
                Data = await pay.ToListAsync(),
                Count = await pay.CountAsync()
            };
        }

        public async Task<QueryResults<PaymentDto>> AddPayment(PaymentDto paymentDto)
        {
            /*
            var createdPay = paymentDto.ConvertToModel();
            createdPay.Id = new Guid();

            await db.Payments.AddAsync(createdPay);
            await db.SaveChangesAsync();
            return new QueryResults<PaymentDto>
            {
                Data = createdPay.ConvertToDto()
            };*/
        }

        public async Task<QueryResults<PaymentDto>> ChangePayment(PaymentDto Payment, Guid id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task DeletePayment(Guid PaymentId)
        {
            var foundPay = db.Payments.GetByIdAsync(PaymentId).Result ?? throw new NotFoundApiException();
            db.Payments.Remove(foundPay);
            await db.SaveChangesAsync();
        }
    }
}
