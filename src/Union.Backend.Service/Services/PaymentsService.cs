using System;
using System.Collections.Generic;
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


        public async Task<QueryResults<PaymentDto>> GetPayment(Guid Id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<List<PaymentDto>>> GetAllPayments()
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<PaymentDto>> AddPayment(PaymentDto Paymentd)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<PaymentDto>> ChangePayment(PaymentDto Payment, Guid id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task DeletePayment(Guid PaymentId)
        {
            throw new WorkInProgressApiException();
        }
    }
}
