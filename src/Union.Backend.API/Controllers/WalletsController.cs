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
        public async Task<WalletQueryResults> GetAllWallets()
        {
            return await service.GetWallet();
        }


        [HttpPut]
        public async Task<WalletQueryResults> AddMessage([FromBody] WalletDto Wallet)
        {

            return await service.ChangeWallet(Wallet);
        }



    }
}
