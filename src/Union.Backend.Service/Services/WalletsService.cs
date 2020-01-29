using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Model.Models;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Results;

namespace Union.Backend.Service.Services
{
    public class WalletsService
    {
        private readonly GardenLinkContext db;
        public WalletsService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }


        public async Task<WalletQueryResults> GetWallet()
        {
            throw new WorkInProgressApiException();
        }


        public async Task<WalletQueryResults> ChangeWallet(WalletDto Wallet)
        {
            throw new WorkInProgressApiException();
        }


    }
}
