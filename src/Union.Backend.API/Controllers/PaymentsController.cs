using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Dtos;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using static Union.Backend.Service.Utils;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentsService paymentService;
        private readonly LeasingsService leasingService;
        public PaymentsController(PaymentsService paymentService, LeasingsService leasingService)
        {
            this.leasingService = leasingService;
            this.paymentService = paymentService;
        }

        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PaymentDto>))]
        public async Task<IActionResult> GetMyPayments()
        {
            try
            {
                var me = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await paymentService.GetMyPayments(me));
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new BadRequestApiException();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentDto))]
        public async Task<IActionResult> GetPaymentById([FromRoute(Name = "id")] Guid paymentId)
        {
            try
            {
                var pay = await paymentService.GetPayment(paymentId);
                var leasing = leasingService.GetLeasing(pay.Data.Leasing).Result.Data;
                var id = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);

                if (leasing.Owner != id && leasing.Renter != id && !IsAdmin(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                    throw new ForbiddenApiException();

                return Ok(pay);
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new BadRequestApiException();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PaymentDto))]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto Payment)
        {
            var result = await paymentService.AddPayment(Payment);
            return Created($"api/Payments/{result.Data.Id}", result);
        }
    }
}
