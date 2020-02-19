using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<QueryResults<WalletDto>> GetWalletByUserId(Guid userId)
        {
            throw new WorkInProgressApiException();
        }


        public async Task<QueryResults<WalletDto>> ChangeWallet(Guid me, Guid my, WalletDto walletDto)
        {
            var wallet = await db.Wallets.GetByIdAsync(my) ?? throw new NotFoundApiException();
            if (!wallet.OfUser.Equals(me))
                throw new UnauthorizedAccessException();

            wallet.Balance = walletDto.Balance;
            await db.SaveChangesAsync();
            return new QueryResults<WalletDto>
            {
                Data = wallet.ConvertToDto()
            };
        }
    }
}
