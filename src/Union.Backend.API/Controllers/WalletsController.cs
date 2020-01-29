using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using Union.Backend.Service.Dtos;

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
        public async Task<IActionResult> GetAllWallets()
        {
            return Ok(await service.GetWallet());
        }

        [HttpPut]
        public async Task<IActionResult> AddMessage([FromBody] WalletDto Wallet)
        {
            return Ok(await service.ChangeWallet(Wallet));
        }
    }
}
