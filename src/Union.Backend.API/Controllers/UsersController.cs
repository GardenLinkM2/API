﻿using System;
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
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await service.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute(Name = "id")] Guid userId)
        {
            return Ok(await service.GetUserById(userId));
        }

        [HttpGet("{id}/gardens")]
        public async Task<IActionResult> GetMyGardens([FromRoute(Name = "id")] Guid userId)
        {
            return Ok(await gardensService.GetMyGardens(userId));
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetMe(myId));
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
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.ChangeUser(id, user));
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
        [Authorize(PermissionType.Admin)]
        public async Task<IActionResult> ChangeUser([FromRoute(Name = "id")] Guid userId, [FromBody] UserDto user)
        {
            return Ok(await service.ChangeUser(userId, user));
        }

        [HttpDelete("{id}")]
        [Authorize(PermissionType.Admin)]
        public async Task DeleteUser([FromRoute(Name = "id")] Guid userId)
        {
            await service.DeleteUser(userId);
        }

        [HttpDelete("me")]
        public async Task DeleteMe()
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                await service.DeleteUser(id);
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

        [HttpPost("{id}/photograph")]
        public async Task<IActionResult> Photograph([FromRoute(Name = "id")] Guid id, [FromBody] PhotoDto photo)
        {
            return Created("NIK", await service.Photograph(id, photo));
        }
    }
}
