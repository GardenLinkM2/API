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
    public class UsersService
    {
        private readonly GardenLinkContext db;
        public UsersService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        private async Task<User> GetUserEntity(Guid id)
        {
            try
            {
                return await db.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserQueryResults> GetUser(Guid userId)
        {
            var user = await GetUserEntity(userId) ?? throw new NotFoundApiException();

            return new UserQueryResults()
            {
                Data = new UserDto() {
                    Id = user.Id,
                    Name = user.Name,
                    FirstName = user.FirstName,
                    Mail = user.Mail,
                    PhoneNumber = user.PhoneNumber,
                    Photos = user.Photos,
                    Wallet = user.Wallet
                }
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
            user.Inscription = DateTime.Now;

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

            foundUser.Name = user.Name;
            foundUser.FirstName = user.FirstName;
            foundUser.Mail = user.Mail;
            foundUser.PhoneNumber = user.PhoneNumber;
            foundUser.Photos = user.Photos;
            foundUser.Password = user.Password;
            
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
