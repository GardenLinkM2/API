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
    [Route("api/users/")]
    [ApiController]
    public class UserScoresController : ControllerBase
    {
        private readonly ScoresService service;
        public UserScoresController(ScoresService service)
        {
            this.service = service;
        }

        [HttpGet("{id}/score")]
        public async Task<IActionResult> GetScore([FromRoute(Name = "id")] Guid UserId)
        {
            return Ok(await service.GetScore(UserId));
        }


        [HttpPost("{id}/score")] //TODO
        public async Task<IActionResult> AddScore([FromRoute(Name = "id")] Guid UserId, [FromBody] ScoreDto Score)
        {
            return Created("TODO", await service.AddScore(Score, UserId));
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
                /* modifier dto 
                   if (score.Data.Rater == id ||  Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                     {
                     await service.DeleteScore(ScoreId);
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
