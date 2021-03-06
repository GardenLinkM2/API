﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Dtos;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using static Union.Backend.Service.Utils;

namespace Union.Backend.API.Controllers
{
    [Route("api/Users")]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await service.GetAllUsers());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<IActionResult> GetUser([FromRoute(Name = "id")] Guid userId)
        {
            return Ok(await service.GetUserById(userId));
        }

        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var myId = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
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

        [HttpGet("{id}/gardens")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GardenDto>))]
        public async Task<IActionResult> GetGardensByUserId([FromRoute(Name = "id")] Guid userId)
        {
            return Ok(await gardensService.GetGardensByUserId(userId));
        }

        [HttpGet("me/gardens")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GardenDto>))]
        public async Task<IActionResult> GetMyGardens()
        {
            try
            {
                var myId = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return await GetGardensByUserId(myId);
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

        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<IActionResult> ChangeMe([FromBody] UserDto user)
        {
            try
            {
                var id = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
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

        [HttpDelete("me")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteMe()
        {
            try
            {
                var id = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                await service.DeleteUser(id);
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

        [HttpPut("me/photo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PhotoDto))]
        public async Task<IActionResult> Photograph([FromBody] PhotoDto photo)
        {
            try
            {
                var me = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.Photograph(me, photo));
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
