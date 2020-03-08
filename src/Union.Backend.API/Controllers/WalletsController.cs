using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Auth;
using System.Net;
using Union.Backend.Service.Exceptions;
using System;
using Microsoft.AspNetCore.Http;

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
                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
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
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);

                return Ok(await service.ChangeWallet(myId, 
                                                     walletId, 
                                                     dto,
                                                     Utils.IsAdmin(Request.Headers[HttpRequestHeader.Authorization.ToString()])));
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
