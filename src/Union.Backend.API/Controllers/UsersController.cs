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
        [Authorize(PermissionType.All)]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await service.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute(Name = "id")] Guid userId)
        {
            return Ok(await service.GetUser(userId));
        }

        [HttpGet("{id}/gardens")]
        public async Task<IActionResult> GetGardens([FromRoute(Name = "id")] Guid userId)
        {
            return Ok(await gardensService.GetGardensByUser(userId));
        }

        [HttpGet("me")]
        [Authorize(PermissionType.All)] //TEMP
        public async Task<IActionResult> GetMe()
        {
            //Example
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                throw new WorkInProgressApiException();
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new BadRequestApiException();
            }
            //TODO
            //return await service.GetUser(GET_IDFROM_TOKEN);
            //TODO gestion http404
        }

        [HttpPost]
        [Authorize(PermissionType.Admin)]
        public async Task<IActionResult> AddUser([FromBody] UserDto user)
        {
            var result = await service.AddUser(user, Guid.NewGuid());
            return Created($"/api/users/{result.Data.Id}", result);
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
