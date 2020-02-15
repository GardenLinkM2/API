using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<User> GetUserById(Guid id)
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

        public async Task<QueryResults<UserDto>> GetUser(Guid userId)
        {
            var user = await GetUserById(userId) ?? throw new NotFoundApiException();

            return new QueryResults<UserDto>
            {
                Data = user.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<UserDto>>> GetAllUsers()
        {
            var users = db.Users
                .Include(u => u.Photos)
                .Select(u => u.ConvertToDto());
            return new QueryResults<List<UserDto>>
            {
                Data = await users.ToListAsync(),
                Count = await users.CountAsync()
            };
        }

        public async Task<QueryResults<UserDto>> AddUser(UserDto userDto, Guid id)
        {
            userDto.Id = id;

            var createdUser = userDto.ConvertToModel();
            createdUser.Inscription = DateTime.Now;

            await db.Users.AddAsync(createdUser);
            await db.SaveChangesAsync();
            return new QueryResults<UserDto>
            {
                Data = createdUser.ConvertToDto()
            };
        }

        public async Task<QueryResults<UserDto>> ChangeUser(Guid id, UserDto user)
        {
            var foundUser = GetUserById(id).Result ?? throw new Exception();

            foundUser.LastName = user.LastName;
            foundUser.FirstName = user.FirstName;
            foundUser.Photos = user.Photos.Select(p => p.ConvertToModel<User>(id)).ToList();
            
            db.Users.Update(foundUser);
            await db.SaveChangesAsync();
            return new QueryResults<UserDto>
            {
                Data = new UserDto { Id = foundUser.Id }
            };
        }

        public async Task DeleteUser(Guid userId)
        {
            var foundUser = GetUserById(userId).Result;
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
