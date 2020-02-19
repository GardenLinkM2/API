using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Union.Backend.Service.Auth;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Services;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeasingController : ControllerBase
    {
        private readonly LeasingsService service;
        public LeasingController(LeasingsService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeasings()
        {
            return Ok(await service.GetAllLeasings());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeasingById([FromRoute(Name = "id")] Guid LeasingId)
        {
            return Ok(await service.GetLeasing(LeasingId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeasing([FromBody] LeasingDto Leasing)
        {
            var result = await service.AddLeasing(Leasing);
            return Created($"/api/Leasing/{result.Data.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeLeasing([FromRoute(Name = "id")] Guid LeasingId, [FromBody] LeasingDto Leasing)
        {
            try
            {

                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var leasing = service.GetLeasing(LeasingId);
                if (leasing.Result.Data.Owner != id || !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    return Forbid();
                }

                return Ok(await service.ChangeLeasing(Leasing, LeasingId));
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

        [HttpDelete("{id}")]
        public async Task DeleteLeasing([FromRoute(Name = "id")] Guid LeasingId)
        {
            try
            {


                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var leasing = service.GetLeasing(LeasingId);
                if (leasing.Result.Data.Owner != id || !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    throw new ForbidenException();
                }


                await service.DeleteLeasing(LeasingId);

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
