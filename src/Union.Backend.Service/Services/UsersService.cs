using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Model.Models;
using Union.Backend.Service.Dtos;
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

        private async Task<User> GetUserEntity(Guid id)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
        }

        public async Task<UserQueryResults> GetUser(Guid userId)
        {
            var user = GetUserEntity(userId).Result;
            if (user == null)
            {
                throw new Exception();
            }
            return new UserQueryResults()
            {
                Data = new UserDto() { Id = user.Id }
            };
        }

        public async Task<UsersQueryResults> GetAllUsers()
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

        public async Task<UserQueryResults> AddUser(User user)
        {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return new UserQueryResults()
            {
                Data = new UserDto { Id = user.Id }
            };
        }

        public async Task<UserQueryResults> ChangeUser(Guid id, User user)
        {
            var foundUser = GetUserEntity(id).Result;
            if (foundUser == null)
            {
                throw new Exception();
            }
            // todo change common foundUser properties by thoses of user
            db.Users.Update(foundUser);
            await db.SaveChangesAsync();
            return new UserQueryResults()
            {
                Data = new UserDto { Id = foundUser.Id }
            };
        }

        public async Task DeleteUser(Guid userId)
        {
            var foundUser = GetUserEntity(userId).Result;
            if (foundUser == null)
            {
                throw new Exception();
            }
            db.Users.Remove(foundUser);
            await db.SaveChangesAsync();
        }
    }
}
