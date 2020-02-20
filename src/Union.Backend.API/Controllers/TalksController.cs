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
    public class TalksController : ControllerBase
    {
        private readonly TalksService service;
        public TalksController(TalksService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTalks()
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetAllTalks(id));

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
        public async Task<IActionResult> GetTalk([FromRoute(Name = "id")] Guid talkId)
        {
            try
            {
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetTalk(myId, talkId));
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
        public async Task<IActionResult> CreateTalk([FromBody] TalkDto talkDto)
        {
            try
            {
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var talk = (await service.AddTalk(talkDto, myId)).Data;
                
                return Created($"{Request.Path.Value}/{talk.Id}", talk);
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
        public async Task<IActionResult> AddMessage([FromRoute(Name = "id")] Guid TalkId, [FromBody] MessageDto message)
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var talk = await service.GetTalk(id, TalkId);

                if (talk.Data.Sender != id && talk.Data.Receiver != id && !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    return Forbid();
                }
                return Created("TODO", await service.AddMessage(message, TalkId));

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
        public async Task DeleteTalk([FromRoute(Name = "id")] Guid TalkId)
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var talk = await service.GetTalk(id, TalkId);

                if (talk.Data.Sender == id || talk.Data.Receiver == id || Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    await service.DeleteTalk(TalkId);
                }
                else
                {
                    throw new ForbidenApiException();
                }

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