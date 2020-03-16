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
using Microsoft.AspNet.OData.Query;
using Union.Backend.Model.Models;
using static Union.Backend.Service.Utils;

namespace Union.Backend.API.Controllers
{
    [Route("api/Gardens")]
    [ApiController]
    public class GardensController : ControllerBase
    {
        private readonly GardensService service;
        public GardensController(GardensService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize(PermissionType.All)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GardenDto>))]
        public async Task<IActionResult> SearchGardens(ODataQueryOptions<Garden> options, double? longi, double? lati, int? dist)
        {
            return Ok(await service.SearchGardens(options, longi, lati, dist));
        }

        [HttpGet("{id}")]
        [Authorize(PermissionType.All)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GardenDto))]
        public async Task<IActionResult> GetGardenById([FromRoute(Name = "id")] Guid GardenId)
        {
            return Ok(await service.GetGardenById(GardenId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GardenDto))]
        public async Task<IActionResult> CreateGarden([FromBody] GardenDto garden)
        {
            try
            {
                var me = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var result = await service.AddGarden(me, garden);
                return Created($"/api/Gardens/{result.Data.Id}", result);
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

        [HttpPost("coordinates")]
        [Authorize(PermissionType.All)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Coordinates))]
        public IActionResult GetCoordinates([FromBody] NullableLocationDto location)
        {
            var (longitude, latitude) = service.GetCoordinates(location);
            return Ok(new { longitude, latitude });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GardenDto))]
        public async Task<IActionResult> UpdateGarden([FromRoute(Name = "id")] Guid gardenId, [FromBody] GardenDto dto)
        {
            try
            {
                var me = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.ChangeGarden(me, gardenId, dto));
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

        [HttpPost("{id}/report")]
        [ProducesResponseType(StatusCodes.Status201Created, Type=typeof(ReportDto))]
        public async Task<IActionResult> ReportGarden([FromRoute(Name = "id")] Guid gardenId, [FromBody] ReportDto dto)
        {
            try
            {
                var me = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Created("/", await service.ReportGarden(me, gardenId, dto));
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
        public async Task<IActionResult> DeleteGarden([FromRoute(Name = "id")] Guid gardenId)
        {
            try
            {
                var me = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var garden = await service.GetGardenById(gardenId);
                if (!garden.Data.Owner.Equals(me) && !IsAdmin(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                    throw new ForbiddenApiException();

                await service.DeleteGarden(gardenId);
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
