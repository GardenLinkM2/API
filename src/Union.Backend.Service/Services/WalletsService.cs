using Microsoft.EntityFrameworkCore;
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
        private readonly UsersService userService;
        public WalletsService(GardenLinkContext gardenLinkContext, UsersService userService)
        {
            db = gardenLinkContext;
            this.userService = userService;
        }

        public async Task<QueryResults<WalletDto>> GetWalletByUserId(Guid userId)
        {
            var user = await userService.GetMe(userId) ?? throw new NotFoundApiException();

            return new QueryResults<WalletDto>
            {
                Data = user.Data.Wallet
            };
        }

        public async Task<QueryResults<WalletDto>> ChangeWallet(Guid me, Guid walletId, WalletDto walletDto, bool isAdmin)
        {
            var wallet = await db.Wallets.Include(w => w.OfUser)
                                           .ThenInclude(u => u.AsRenter)
                                               .ThenInclude(l => l.Garden)
                                                   .ThenInclude(g => g.Criteria)
                                         .GetByIdAsync(walletId) ?? throw new NotFoundApiException();

            if (!wallet.OfUser.Id.Equals(me) && !isAdmin)
                throw new ForbiddenApiException();

            wallet.TrueBalance += walletDto.Balance;
            await db.SaveChangesAsync();

            return new QueryResults<WalletDto>
            {
                Data = wallet.ConvertToDto()
            };
        }
    }
}
