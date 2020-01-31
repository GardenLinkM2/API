using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using System;
using Union.Backend.Service.Dtos;

namespace Union.Backend.API.Controllers
{
    [Route("api/gardens/")]
    [ApiController]
    public class GardenScoresController : ControllerBase
    {
        private readonly ScoresService service;
        public GardenScoresController(ScoresService service)
        {
            this.service = service;
        }

        [HttpGet("{id}/score")]
        public async Task<IActionResult> GetScore([FromRoute(Name = "id")] Guid GardenId)
        {
            return Ok(await service.GetScore(GardenId));
        }


        [HttpPost("{id}/score")] //TODO
        public async Task<IActionResult> AddScore([FromRoute(Name = "id")] Guid GardenId, [FromBody] ScoreDto Score)
        {
            return Created("TODO", await service.AddScore(Score, GardenId));
        }

        [HttpPost("score/{id}/report")] //TODO
        public async Task<IActionResult> ReportScore([FromRoute(Name = "id")] Guid ScoreId, [FromBody] ScoreDto Score)
        {
            return Created("TODO", await service.ReportScore(ScoreId, Score));
        }

        [HttpDelete("score/{id}")]
        public async Task DeleteScore([FromRoute(Name = "id")] Guid ScoreId)
        {
            await service.DeleteScore(ScoreId);
        }

    }
}
