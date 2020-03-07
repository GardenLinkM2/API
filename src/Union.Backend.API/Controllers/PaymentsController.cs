using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Auth;
using System.Net;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentsService service;
        private readonly LeasingsService leasingService;
        public PaymentsController(PaymentsService service, LeasingsService leasingService)
        {
            this.leasingService = leasingService;
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetAllPayments(id));
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

        [HttpGet("{id}/user")]
        [Authorize(PermissionType.Admin)]
        public async Task<IActionResult> GetAllPaymentsByUserId()
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetAllPayments(id));

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
        public async Task<IActionResult> GetPaymentById([FromRoute(Name = "id")] Guid PaymentId)
        {
            try
            {
                var pay = await service.GetPayment(PaymentId);
                var leasing = leasingService.GetLeasing(pay.Data.Leasing).Result.Data;
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);

                if (leasing.Owner != id || leasing.Renter != id || !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    return Forbid();
                }

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
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto Payment)
        {
            var result = await service.AddPayment(Payment);
            return Created($"api/Payments/{result.Data.Id}", result);
        }

        [HttpDelete("{id}")]
        [Authorize(PermissionType.Admin)]
        public async Task DeletePayment([FromRoute(Name = "id")] Guid PaymentId)
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var pay = await service.GetPayment(PaymentId);
                var leasing = leasingService.GetLeasing(pay.Data.Leasing).Result.Data;

                if (leasing.Owner != id || !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    await service.DeletePayment(PaymentId);
                }
                else
                {
                    throw new ForbidenApiException();
                }
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
    }
}
