﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Dtos;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using static Union.Backend.Service.Utils;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalksController : ControllerBase
    {
        private readonly TalksService service;
        public TalksController(TalksService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TalkDto>))]
        public async Task<IActionResult> GetMyTalks()
        {
            try
            {
                var id = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetMyTalks(id));
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TalkDto))]
        public async Task<IActionResult> GetTalkById([FromRoute(Name = "id")] Guid talkId)
        {
            try
            {
                var myId = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetTalkById(myId, talkId));
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TalkDto))]
        public async Task<IActionResult> CreateTalk([FromBody] TalkDto talkDto)
        {
            try
            {
                var myId = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var talk = await service.CreateTalk(myId, talkDto);
                
                return Created($"{Request.Path.Value}/{talk.Data.Id}", talk);
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

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MessageDto))]
        public async Task<IActionResult> PostMessageToTalk([FromRoute(Name = "id")] Guid talkId, [FromBody] MessageDto message)
        {
            try
            {
                var myId = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var created = await service.PostMessageToTalk(myId, talkId, message);

                var paths = Request.Path.Value.Split('/');
                return Created($"{string.Join('/', paths.Take(paths.Length - 1))}/{created.Data.Id}", created);
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
        public async Task<IActionResult> DeleteTalk([FromRoute(Name = "id")] Guid talkId)
        {
            try
            {
                var id = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                await service.DeleteTalk(id, talkId);
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