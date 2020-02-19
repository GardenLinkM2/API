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

                /* modifier dto
                if (garden.Data.Owner != id &&  garden.Data.Owner == id &&  !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    return Forbid();
                }*/

                return Created("TODO", await service.GetAllTalks(id));

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
        public async Task<IActionResult> GetTalk([FromRoute(Name = "id")] Guid TalkId)
        {

            try
            {
                var talk = await service.GetTalk(TalkId);
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);

                /* modifier dto
                if (talk.Data.Sender != id &&  garden.Data.Receiver != id &&  !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    return Forbid();
                }*/

                return Ok(talk);

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
        public async Task<IActionResult> CreateTalk([FromBody] TalkDto Talk)
        {
            return Created("TODO", await service.AddTalk(Talk));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddMessage([FromRoute(Name = "id")] Guid TalkId, [FromBody] MessageDto message)
        {
            try
            {
                var talk = await service.GetTalk(TalkId);
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                /* modifier dto
                    if (talk.Data.Sender != id &&  garden.Data.Receiver != id &&  !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                    {
                        return Forbid();
                    }*/
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
                var score = await service.GetTalk(TalkId);
                /* modifier dto 
                 * if (talk.Data.Sender == id ||  garden.Data.Receiver == id || Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                    {
                        await service.DeleteTalk(TalkId);
                    }*/

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