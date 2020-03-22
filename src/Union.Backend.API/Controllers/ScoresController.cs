using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Dtos;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static Union.Backend.Service.Utils;

namespace Union.Backend.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly ScoresService service;
        public ScoresController(ScoresService service)
        {
            this.service = service;
        }

        [HttpGet("Users/me/score")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ScoreDto>))]
        public async Task<IActionResult> GetScoresOnMe()
        {
            try
            {
                var me = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetUserScores(me));
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

        [HttpGet("Gardens/{id}/score")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ScoreDto>))]
        public async Task<IActionResult> GetScores([FromRoute(Name = "id")] Guid ratedId)
        {
            return Ok(await service.GetGardenScores(ratedId));
        }

        [HttpGet("Users/{id}/score")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ScoreDto>))]
        public async Task<IActionResult> GetUserScores([FromRoute(Name = "id")] Guid UserId)
        {
            return Ok(await service.GetUserScores(UserId));
        }

        [HttpPost("Gardens/{id}/score")]
        [HttpPost("Users/{id}/score")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ScoreDto))]
        public async Task<IActionResult> AddScore([FromRoute(Name = "id")] Guid ratedId, [FromBody] ScoreDto score)
        {
            try
            {
                var me = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var result = await service.AddScore(me, ratedId, score);
                return Created($"/api/gardens/{result.Data.Id}/score", result);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ReportScore([FromRoute(Name = "id")] Guid scoreId)
        {
            await service.ReportScore(scoreId);
            return NoContent();
        }

        [HttpDelete("Score/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteScore([FromRoute(Name = "id")] Guid scoreId)
        {
            try
            {
                var id = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var score = await service.GetScoreById(scoreId);
                if (score.Data.Rater == id || IsAdmin(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                    await service.DeleteScore(scoreId);
                else
                    throw new ForbiddenApiException();

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
