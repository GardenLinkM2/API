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
        public async Task<ScoreQueryResults> GetScore([FromRoute(Name = "id")] Guid GardenId)
        {
            return await service.GetScore(GardenId);
        }


        [HttpPost("{id}/score")]
        public async Task<ScoreQueryResults> AddScore([FromRoute(Name = "id")] Guid GardenId, [FromBody] ScoreDto Score)
        {
            return await service.AddScore(Score, GardenId);
        }

        [HttpPost("score/{id}/report")]
        public async Task<ScoreQueryResults> ReportScore([FromRoute(Name = "id")] Guid ScoreId, [FromBody] ScoreDto Score)
        {

            return await service.ReportScore(ScoreId, Score);
        }

        [HttpDelete("score/{id}")]
        public async Task DeleteScore([FromRoute(Name = "id")] Guid ScoreId)
        {
            await service.DeleteScore(ScoreId);
        }

    }
}
