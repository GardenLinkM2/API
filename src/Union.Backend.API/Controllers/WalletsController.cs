using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Auth;
using System.Net;
using Union.Backend.Service.Exceptions;
using System;

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

        [HttpGet]
        [Authorize(PermissionType.Admin)]
        public async Task<IActionResult> GetAllWallets()
        {
            return Ok(await service.GetWallet());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddMessage([FromRoute(Name = "id")] Guid walletId, [FromBody] WalletDto Wallet)
        {
            try
            {
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.ChangeWallet(myId, walletId, Wallet));
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
