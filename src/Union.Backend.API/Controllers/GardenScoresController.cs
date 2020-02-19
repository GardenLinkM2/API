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
    [Route("api/gardens/")]
    [ApiController]
    public class GardenScoresController : ControllerBase
    {
        private readonly ScoresService service;
        private readonly GardensService GardenService;
        public GardenScoresController(ScoresService service)
        {
            this.service = service;
        }

        [HttpGet("{id}/score")]
        public async Task<IActionResult> GetScore([FromRoute(Name = "id")] Guid GardenId)
        {
            return Ok(await service.GetScore(GardenId));
        }


        [HttpPost("{id}/score")]
        public async Task<IActionResult> AddScore([FromRoute(Name = "id")] Guid GardenId, [FromBody] ScoreDto Score)
        {
            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var garden = await GardenService.GetGardenById(GardenId);
                if (garden.Data.Owner != id && garden.Data.Owner != id && !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    return Forbid();
                }
                var result = await service.AddScore(Score, GardenId);
                return Created($"api/gardens/{result.Data.Id}/score", result);

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

        [HttpPost("score/{id}/report")]
        public async Task<IActionResult> ReportScore([FromRoute(Name = "id")] Guid ScoreId, [FromBody] ScoreDto Score)
        {
            return Ok(await service.ReportScore(ScoreId, Score));
        }

        [HttpDelete("score/{id}")]
        public async Task DeleteScore([FromRoute(Name = "id")] Guid ScoreId)
        {

            try
            {
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var score = await service.GetScore(ScoreId);
                if (score.Data.Rater == id || Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    await service.DeleteScore(ScoreId);
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
