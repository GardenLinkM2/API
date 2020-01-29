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
    public class TalksController : ControllerBase
    {
        private readonly TalksService service;
        public TalksController(TalksService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<TalksQueryResults> GetAllTalks()
        {
            return await service.GetAllTalks();
        }

        [HttpGet("{id}")]
        public async Task<TalkQueryResults> GetTalk([FromRoute(Name = "id")] Guid UserId)
        {
            return await service.GetTalk(UserId);
        }


        [HttpPost]
        public async Task<TalkQueryResults> CreateTalk([FromBody] TalkDto Talk)
        {
            return await service.AddTalk(Talk);
        }

        [HttpPost("{id}")]
        public async Task<TalkQueryResults> AddMessage([FromRoute(Name = "id")] Guid TalkId, [FromBody] MessageDto message)
        {
            return await service.AddMessage(message, TalkId);
        }



        [HttpDelete("{id}")]
        public async Task DeleteTalk([FromRoute(Name = "id")] Guid TalkId)
        {
            await service.DeleteTalk(TalkId);
        }

    }
}
