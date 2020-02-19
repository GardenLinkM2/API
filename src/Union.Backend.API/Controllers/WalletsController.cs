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
        private readonly UsersService userService;

        public WalletsController(WalletsService service)
        {
            this.service = service;
        }

        [HttpGet("me")]
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
        public async Task<IActionResult> AddMessage([FromRoute(Name = "id")] Guid walletId, [FromBody] WalletDto Wallet)
        {
            try
            {


                var id = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var user = userService.GetUser(id);

                if (user.Result.Data.Wallet.Id != id || !Utils.IsAdminRoleFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]))
                {
                    return Forbid();
                }
                return Ok(await service.ChangeWallet(id, walletId, Wallet));
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
