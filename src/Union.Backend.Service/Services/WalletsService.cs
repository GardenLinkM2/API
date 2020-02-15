﻿using System.Threading.Tasks;
using Union.Backend.Model.DAO;
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

        public async Task<QueryResults<WalletDto>> GetWallet()
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<WalletDto>> ChangeWallet(WalletDto Wallet)
        {
            throw new WorkInProgressApiException();
        }
    }
}
