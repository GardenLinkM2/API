using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Dtos;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService service;
        private readonly GardensService gardensService;
        public UsersController(UsersService service, GardensService gardensService)
        {
            this.service = service;
            this.gardensService = gardensService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await service.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute(Name = "id")] Guid userId)
        {
            try
            {
                return Ok(await service.GetUser(userId));
            }
            catch (NotFoundApiException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/gardens")]
        public async Task<IActionResult> GetGardens([FromRoute(Name = "id")] Guid userId)
        {
            return Ok(await gardensService.GetGardensByUser(userId));
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            //TODO
            //return await service.GetUser(GET_IDFROM_TOKEN);
            throw new WorkInProgressApiException();
            //TODO gestion http404
        }

        [HttpPost] //TODO
        public async Task<IActionResult> AddUser([FromBody] UserDto user)
        {
            return Created("TODO", await service.AddUser(user));
        }

        [HttpPut("me")]
        public async Task<IActionResult> ChangeMe([FromBody] UserDto user)
        {
            //TODO var id = getuserIdFromToken();
            //return await service.ChangeUser(user, id);
            throw new WorkInProgressApiException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeUser([FromRoute(Name = "id")] Guid userId, [FromBody] UserDto user)
        {
            return Ok(await service.ChangeUser(userId, user));
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
            throw new WorkInProgressApiException();
        }

        [HttpPost("{id}/photograph")]
        public async Task<IActionResult> Photograph([FromRoute(Name = "id")] Guid id, [FromBody] PhotoDto photo)
        {
            return Created("NIK", await service.Photograph(id, photo));
        }
    }
}
