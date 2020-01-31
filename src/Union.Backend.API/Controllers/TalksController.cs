using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using System;
using Union.Backend.Service.Dtos;

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
            return Ok(await service.GetAllTalks());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTalk([FromRoute(Name = "id")] Guid UserId)
        {
            return Ok(await service.GetTalk(UserId));
        }


        [HttpPost] //TODO
        public async Task<IActionResult> CreateTalk([FromBody] TalkDto Talk)
        {
            return Created("TODO", await service.AddTalk(Talk));
        }

        [HttpPost("{id}")] //TODO
        public async Task<IActionResult> AddMessage([FromRoute(Name = "id")] Guid TalkId, [FromBody] MessageDto message)
        {
            return Created("TODO", await service.AddMessage(message, TalkId));
        }

        [HttpDelete("{id}")]
        public async Task DeleteTalk([FromRoute(Name = "id")] Guid TalkId)
        {
            await service.DeleteTalk(TalkId);
        }

    }
}
