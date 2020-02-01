using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using System;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;

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
            //return Ok(await service.GetAllFriends(GUID)); TODO
            throw new WorkInProgressApiException();
        }

        [HttpPost] //TODO
        public async Task<IActionResult> CreateFriendDemand([FromBody] DemandDto demand)
        {
            return Created("TODO", await service.AddContactDemand(demand));
        }

        [HttpPost("{id}/Accept")] //TODO
        public async Task<IActionResult> AcceptDemand([FromRoute(Name = "id")] Guid friendId)
        {
            return Created("TODO", await service.CreateContact(friendId));
        }

        [HttpPost("{id}/decline")] //TODO
        public async Task DeclineDemand([FromRoute(Name = "id")] Guid friendId)
        {
            await service.DeleteDemand(friendId);
        }

        [HttpDelete("{id}")]
        public async Task DeleteFriend([FromRoute(Name = "id")] Guid userId)
        {
            await service.DeleteContact(userId);
        }
    }
}
