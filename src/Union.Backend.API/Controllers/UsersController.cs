using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Model.Models;
using Union.Backend.Service.Services;
using Union.Backend.Service.Results;
using System;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService service;
        public UsersController(UsersService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<UsersQueryResults> GetAllUsers()
        {
            return await service.GetAllUsers();
        }

        [HttpGet("{id}")]
        public async Task<UserQueryResults> GetUser([FromRoute(Name = "id")] Guid userId)
        {
            //TODO gestion http404
            return await service.GetUser(userId);
        }

        [HttpGet("me")]
        public async Task<UserQueryResults> GetMe()
        {
            //TODO
            //return await service.GetUser(GET_IDFROM_TOKEN);
            throw new NotImplementedException("nik");
            //TODO gestion http404
        }

        [HttpPost]
        public async Task<UserQueryResults> AddUser([FromBody] User user)
        {
            return await service.AddUser(user);
        }

        [HttpPut("me")]
        public async Task<UserQueryResults> ChangeMe([FromBody] User user)
        {
            //TODO var id = getuserIdFromToken();
            //return await service.ChangeUser(user, id);
            throw new NotImplementedException("nik");
        }

        [HttpPut("{id}")]
        public async Task<UserQueryResults> ChangeUser([FromRoute(Name = "id")] Guid userId, [FromBody] User user)
        {

            return await service.ChangeUser(userId, user);
        }

        [HttpDelete("{id}")]
        public async Task DeleteUser([FromRoute(Name = "id")] Guid userId)
        {
            await service.DeleteUser(userId);
        }

        [HttpDelete("me")]
        public async Task DeleteMe()
        {
            //TODO var id = getuserIdFromToken();
            //return await service.DeleteUser(id);
            throw new NotImplementedException("nik");
        }
    }
}
