using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using System;
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
        public async Task<IActionResult> GetAllPayments()
        {
            return Ok(await service.GetAllPayments());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById([FromRoute(Name = "id")] Guid PaymentId)
        {
            return Ok(await service.GetPayment(PaymentId));
        }

        [HttpPost] //TODO
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto Payment)
        {
            return Created("TODO", await service.AddPayment(Payment));
        }

        [HttpDelete("{id}")]
        public async Task DeletePayment([FromRoute(Name = "id")] Guid PaymentId)
        {
            await service.DeletePayment(PaymentId);
        }

    }
}
