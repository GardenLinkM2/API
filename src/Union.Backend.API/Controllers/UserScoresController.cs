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
    [Route("api/Users")]
    [ApiController]
    public class UserScoresController : ControllerBase
    {
        private readonly ScoresService service;
        public UserScoresController(ScoresService service)
        {
            this.service = service;
        }

        [HttpGet("{id}/score/rater")]
        public async Task<IActionResult> GetScoreUserRater([FromRoute(Name = "id")] Guid UserId)
        {
            return Ok(await service.GetScoresByUserRater(UserId));
        }

        [HttpGet("{id}/score/rated")]
        public async Task<IActionResult> GetScoreRatedUser([FromRoute(Name = "id")] Guid UserId)
        {
            return Ok(await service.GetScoresByUserRated(UserId));
        }

        [HttpPost("{id}/score")]
        public async Task<IActionResult> AddScore([FromRoute(Name = "id")] Guid UserId, [FromBody] ScoreDto Score)
        {
            var result = await service.AddScore(Score, UserId);
            return Created($"/api/Users/{result.Data.Id}/score", result);
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
                var score = await service.GetScoreById(ScoreId);

                if (score.Data.Rater == id || Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    await service.DeleteScore(ScoreId);
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
