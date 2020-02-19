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
    public class GardensController : ControllerBase
    {
        private readonly GardensService service;
        public GardensController(GardensService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize(PermissionType.All)]
        public async Task<IActionResult> GetAllGardens()
        {
            return Ok(await service.GetAllGardens());
        }

        [HttpGet("search/{params}")]
        [Authorize(PermissionType.All)]
        public async Task<IActionResult> GetGardensByParam([FromRoute(Name = "params")] Guid GardenId)
        {
            //return Ok(await service.GetGardensByParams());
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [Authorize(PermissionType.All)]
        public async Task<IActionResult> GetGardenById([FromRoute(Name = "id")] Guid GardenId)
        {
            return Ok(await service.GetGardenById(GardenId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGarden([FromBody] GardenDto Garden)
        {
            var result = await service.AddGarden(Garden);
            return Created($"/api/Gardens/{result.Data.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGarden([FromRoute(Name = "id")] Guid GardenId, [FromBody] GardenDto garden)
        {

            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var jardin = await service.GetGardenById(GardenId);
                /*
                if (jardin.Data.Owner != id && !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    return Forbid();
                }*/
                return Ok(await service.ChangeGarden(garden, GardenId));
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

        [HttpPut("{id}/description")]
        public async Task<IActionResult> UpdateGardenDescription([FromRoute(Name = "id")] Guid GardenId, [FromBody] DescriptionDto desc)
        {

            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var jardin = await service.GetGardenById(GardenId);
                /*Modifeir GardenDto pur remplacer  Owner entity par guid
                if (jardin.Data.Owner != id && !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    return Forbid();
                }*/
                return Ok(await service.ChangeGardenDescription(desc, GardenId));
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

        [HttpPut("{id}/validation")]
        public async Task<IActionResult> UpdateGardenValidation([FromRoute(Name = "id")] Guid GardenId, [FromBody] ValidationDto valid)
        {

            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var jardin = await service.GetGardenById(GardenId);
                /*Modifeir GardenDto pur remplacer  Owner entity par guid
                if (jardin.Data.Owner != id && !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    return Forbid();
                }*/
                return Ok(await service.ChangeGardenValidation(valid, GardenId));
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
        public async Task DeleteGarden([FromRoute(Name = "id")] Guid GardenId)
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var jardin = await service.GetGardenById(GardenId);
                /*Modifeir GardenDto pur remplacer  Owner entity par guid
                if(jardin.Data.Owner == id || Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]) ){
                    await service.DeleteGarden(GardenId);
                }
                */
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
