using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Auth;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

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

        [HttpGet]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PaymentDto>))]
        public async Task<IActionResult> GetAllPayments()
        {
            return Ok(await paymentService.GetAllPayments());
        }

        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PaymentDto>))]
        public async Task<IActionResult> GetMyPayments()
        {
            try
            {
                var me = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
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
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);

                if (leasing.Owner != id && leasing.Renter != id && !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                    throw new ForbidenApiException();

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

        [HttpDelete("{id}")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePayment([FromRoute(Name = "id")] Guid paymentId)
        {
            await paymentService.DeletePayment(paymentId);
            return NoContent();
        }
    }
}
