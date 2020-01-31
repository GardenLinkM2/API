using Microsoft.EntityFrameworkCore;
using System;
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


        public async Task<PaymentQueryResults> GetPayment(Guid Id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<PaymentsQueryResults> GetAllPayments()
        {
            throw new WorkInProgressApiException();
        }

        public async Task<PaymentQueryResults> AddPayment(PaymentDto Paymentd)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<PaymentQueryResults> ChangePayment(PaymentDto Payment, Guid id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task DeletePayment(Guid PaymentId)
        {
            throw new WorkInProgressApiException();
        }
    }
}
