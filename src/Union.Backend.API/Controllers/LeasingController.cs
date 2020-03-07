using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LeasingDto>))]
        public async Task<IActionResult> GetAllLeasings()
        {
            return Ok(await service.GetAllLeasings());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeasingDto))]
        public async Task<IActionResult> GetLeasingById([FromRoute(Name = "id")] Guid leasingId)
        {
            return Ok(await service.GetLeasing(leasingId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LeasingDto))]
        public async Task<IActionResult> CreateLeasing([FromBody] LeasingDto leasingDto)
        {
            try
            {
                var me = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var result = await service.AddLeasing(me, leasingDto);
                return Created($"/api/Leasing/{result.Data.Id}", result);
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeasingDto))]
        public async Task<IActionResult> ChangeLeasing([FromRoute(Name = "id")] Guid leasingId, [FromBody] LeasingDto leasingDto)
        {
            try
            {
                var me = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var leasing = await service.GetLeasing(leasingId);
                if (leasing.Data.Owner != me && !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteLeasing([FromRoute(Name = "id")] Guid leasingId)
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var leasing = service.GetLeasing(leasingId);
                if (leasing.Result.Data.Owner != id && !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    throw new ForbidenApiException();
                }
                await service.DeleteLeasing(leasingId);

                return NoContent();
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
