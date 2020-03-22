using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Dtos;
using System.Net;
using Union.Backend.Service.Exceptions;
using System;
using Microsoft.AspNetCore.Http;
using static Union.Backend.Service.Utils;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly WalletsService service;

        public WalletsController(WalletsService service)
        {
            this.service = service;
        }


        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WalletDto))]
        public async Task<IActionResult> GetWallet()
        {
            try
            {
                var id = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetWalletByUserId(id));
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WalletDto))]
        public async Task<IActionResult> ChangeWallet([FromRoute(Name = "id")] Guid walletId, [FromBody] WalletDto dto)
        {
            try
            {
                var myId = ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);

                return Ok(await service.ChangeWallet(myId, 
                                                     walletId, 
                                                     dto,
                                                     IsAdmin(Request.Headers[HttpRequestHeader.Authorization.ToString()])));
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
