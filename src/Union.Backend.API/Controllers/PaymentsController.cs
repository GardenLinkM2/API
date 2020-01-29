using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Model.Models;
using Union.Backend.Service.Services;
using Union.Backend.Service.Results;
using System;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Dtos;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentsService service;
        public PaymentsController(PaymentsService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<PaymentsQueryResults> GetAllPayments()
        {
            return await service.GetAllPayments();
        }

        [HttpGet("{id}")]
        public async Task<PaymentQueryResults> GetPaymentById([FromRoute(Name = "id")] Guid PaymentId)
        {
            return await service.GetPayment(PaymentId);
        }

        [HttpPost]
        public async Task<PaymentQueryResults> CreatePayment([FromBody] PaymentDto Payment)
        {
            return await service.AddPayment(Payment);
        }

        [HttpDelete("{id}")]
        public async Task DeletePayment([FromRoute(Name = "id")] Guid PaymentId)
        {
            await service.DeletePayment(PaymentId);
        }

    }
}
