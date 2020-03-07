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

        [HttpGet("me")]
        public async Task<IActionResult> GetAllMyLeasings()
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetAllLeasingsByUserId(id));
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
        public async Task<IActionResult> GetAllUserLeasings([FromRoute(Name = "id")] Guid userId)
        {
            return Ok(await service.GetAllLeasingsByUserId(userId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeasingById([FromRoute(Name = "id")] Guid leasingId)
        {
            return Ok(await service.GetLeasing(leasingId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeasing([FromBody] LeasingDto leasingDto)
        {
            var result = await service.AddLeasing(leasingDto);
            return Created($"/api/Leasing/{result.Data.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeLeasing([FromRoute(Name = "id")] Guid leasingId, [FromBody] LeasingDto leasingDto)
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var leasing = service.GetLeasing(leasingId);
                if (leasing.Result.Data.Owner != id || !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    throw new ForbidenApiException();
                }

                return Ok(await service.ChangeLeasing(leasingId, leasingDto));
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
        public async Task DeleteLeasing([FromRoute(Name = "id")] Guid leasingId)
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var leasing = service.GetLeasing(leasingId);
                if (leasing.Result.Data.Owner != id || !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    throw new ForbidenApiException();
                }
                await service.DeleteLeasing(leasingId);
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
