using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Model.Models;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Services;
using Union.Backend.Service.Results;

namespace Union.Backend.Service.Services
{
    public class UsersService
    {
        private readonly GardenLinkContext db;
        public UsersService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<UserQueryResults> Add(User user)
        {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return new UserQueryResults()
            {
                Data =  new UserDto  { Id = user.Id }
            };
        }

        public async Task<UsersQueryResults> All()
        {
            var users = db.Users
                    .Select(u => new UserDto
                    {
                        Id = u.Id
                    });
            return new UsersQueryResults()
            {
                Data = await users.ToListAsync(),
                Count = await users.CountAsync()
            };
        }
    }
}
