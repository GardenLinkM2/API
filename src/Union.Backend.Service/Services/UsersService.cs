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

        public async Task<QueryResults<UserDto>> GetUser(Guid userId)
        {
            var user = await db.Users
                .Include(u => u.Photo)
                .Include(u => u.Wallet)
                .GetByIdAsync(userId) ?? throw new NotFoundApiException();

            return new QueryResults<UserDto>
            {
                Data = user.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<UserDto>>> GetAllUsers()
        {
            var users = db.Users
                .Include(u => u.Photo)
                .Include(u => u.Wallet)
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
            createdUser.Wallet = new Wallet();
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
            var foundUser = db.Users.GetByIdAsync(id).Result ?? throw new NotFoundApiException();

            foundUser.LastName = user.LastName;
            foundUser.FirstName = user.FirstName;
            foundUser.Photo = user.Photo.ConvertToModel<User>(id);
            
            db.Users.Update(foundUser);
            await db.SaveChangesAsync();
            return new QueryResults<UserDto>
            {
                Data = new UserDto { Id = foundUser.Id }
            };
        }

        public async Task DeleteUser(Guid userId)
        {
            var foundUser = db.Users.GetByIdAsync(userId).Result ?? throw new NotFoundApiException();
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
