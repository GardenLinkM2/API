using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Model.Models;
using Union.Backend.Service.IServices;
using Union.Backend.Service.Results;

namespace Union.Backend.Service.Services
{
    public class UsersService : IUsersService
    {
        private readonly GardenLinkContext db;
        public UsersService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<UsersQueryResults> Add(User user)
        {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return new UsersQueryResults()
            {
                Data = new List<User>() { user }
            };
        }

        public async Task<UsersQueryResults> All()
        {
            return new UsersQueryResults()
            {
                Data = await db.Users.ToListAsync()
            };
        }
    }
}
