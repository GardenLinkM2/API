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
        private readonly UsersService usersService;
        public PaymentsService(GardenLinkContext gardenLinkContext, LeasingsService leasingService, UsersService usersService)
        {
            this.usersService = usersService;
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
                .Where(u => u.Leasing.Owner.Id == userId || u.Leasing.Renter.Id == userId)
                .Select(u => u.ConvertToDto());

            return new QueryResults<List<PaymentDto>>
            {
                Data = await pay.ToListAsync(),
                Count = await pay.CountAsync()
            };
        }

        public async Task<QueryResults<PaymentDto>> AddPayment(PaymentDto paymentDto)
        {
            var leasing = leasingService.GetLeasing(paymentDto.Leasing).Result.Data;
            var owner = usersService.GetUserById(leasing.Owner).Result.Data.ConvertToModel();
            var renter = usersService.GetUserById(leasing.Renter).Result.Data.ConvertToModel();

            var createdPay = paymentDto.ConvertToModel(leasing, owner, renter);
            createdPay.Id = new Guid();

            await db.Payments.AddAsync(createdPay);
            await db.SaveChangesAsync();
            return new QueryResults<PaymentDto>
            {
                Data = createdPay.ConvertToDto()
            };
        }

        public async Task<QueryResults<PaymentDto>> ChangePayment(PaymentDto payment, Guid id)
        {
            var foundPayment = db.Payments.GetByIdAsync(id).Result ?? throw new NotFoundApiException();
            var leasing = leasingService.GetLeasing(payment.Leasing).Result.Data;

            foundPayment.Sum = payment.Sum;
            foundPayment.State = payment.State;
            foundPayment.Leasing = leasing.ConvertToModel((await usersService.GetUserById(leasing.Owner)).Data.ConvertToModel(), (await usersService.GetUserById(leasing.Renter)).Data.ConvertToModel());

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
