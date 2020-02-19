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
    public class FriendsController : ControllerBase
    {
        private readonly FriendsService service;
        public FriendsController(FriendsService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFriends()
        {

            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetAllFriends(id));
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

        [HttpGet("Demands")]
        public async Task<IActionResult> GetAllDemandsForCurrentUser()
        {

            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetAllContactDemands(id));
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

        [HttpGet("Demands/{id}")]
        public async Task<IActionResult> GetDemandsbyId([FromRoute(Name = "id")] Guid demandId)
        {

            return Ok(await service.GetContactDemandById(demandId));

        }


        [HttpPost]
        public async Task<IActionResult> CreateFriendDemand([FromBody] DemandDto demand)
        {
            var result = await service.AddContactDemand(demand);
            return Created($"/api/Friends/Demands/{result.Data.Id}", result);
        }

        [HttpPost("Demands/{id}/Accept")]
        public async Task<IActionResult> AcceptDemand([FromRoute(Name = "id")] Guid demandId)
        {
            try
            {
                /* MODIFIER DEMANDDTO to only expose receiver and sender uuid
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                if(await service.GetContactDemandById(demandId) ==null || await service.GetContactDemandById(demandId).Result.Data.Receiver!=id)
                {
                    return Forbid();
                }
                */
                return Ok(await service.CreateContact(demandId));
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

        [HttpPost("Demands/{id}/Decline")]
        public async Task DeclineDemand([FromRoute(Name = "id")] Guid demandId)
        {

            await service.DeleteDemand(demandId);
        }

        [HttpDelete("{id}")]
        public async Task DeleteFriend([FromRoute(Name = "id")] Guid userId)
        {
            await service.DeleteContact(userId);
        }
    }
}
