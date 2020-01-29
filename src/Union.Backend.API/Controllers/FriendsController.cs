using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Model.Models;
using Union.Backend.Service.Services;
using Union.Backend.Service.Results;
using System;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Dtos;

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
        public async Task<ContactsQueryResults> GetAllFriends()
        {
            //return await service.GetAllUsers();
            throw new NotFoundApiException();
        }

        [HttpPost]
        public async Task<DemandQueryResults> CreateFriendDemand([FromBody] DemandDto demand)
        {
            return await service.AddContactDemand(demand);
        }

        [HttpPost("{id}/Accept")]
        public async Task<ContactQueryResults> AcceptDemand([FromRoute(Name = "id")] Guid friendId)
        {
            return await service.CreateContact(friendId);
        }

        [HttpPost("{id}/decline")]
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
