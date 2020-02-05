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
                Data = user.ConvertToDto()
            };
        }

        public async Task<UsersQueryResults> GetAllUsers()
        {
            var users = db.Users
                .Include(u => u.Photos)
                .Select(u => u.ConvertToDto());
            return new UsersQueryResults()
            {
                Data = await users.ToListAsync(),
                Count = await users.CountAsync()
            };
        }

        public async Task<UserQueryResults> AddUser(UserDto user)
        {
            User createdUser = user.ConvertToModel();
            await db.Users.AddAsync(createdUser);
            await db.SaveChangesAsync();
            return new UserQueryResults()
            {
                Data = createdUser.ConvertToDto()
            };
        }

        public async Task<UserQueryResults> ChangeUser(Guid id, UserDto user)
        {
            var foundUser = GetUserEntity(id).Result ?? throw new Exception();

            foundUser.Name = user.Name;
            foundUser.FirstName = user.FirstName;
            foundUser.Mail = user.Mail;
            foundUser.PhoneNumber = user.PhoneNumber;
            foundUser.Photos = user.Photos.Select(p => p.ConvertToModel<User>(id)).ToList();
            
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

        public async Task<PhotoDto> Photograph(Guid id, PhotoDto dto)
        {
            var photo = dto.ConvertToModel<User>(id);
            db.UserPhotos.Add(photo);
            await db.SaveChangesAsync();
            return photo.ConvertToDto();
        }
    }
}
