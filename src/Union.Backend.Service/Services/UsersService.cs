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

        public async Task<QueryResults<UserDto>> GetUserById(Guid userId)
        {
            var user = await db.Users.Include(u => u.Photo)
                                     .GetByIdAsync(userId) ?? throw new NotFoundApiException();

            return new QueryResults<UserDto>
            {
                Data = user.ConvertToDto()
            };
        }

        public async Task<QueryResults<UserDto>> GetMe(Guid userId)
        {
            var user = await db.Users.Include(u => u.Photo)
                                     .Include(u => u.Wallet)
                                     .GetByIdAsync(userId) ?? throw new NotFoundApiException();

            return new QueryResults<UserDto>
            {
                Data = user.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<UserDto>>> GetAllUsers()
        {
            var users = db.Users.Include(u => u.Photo)
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
            createdUser.Inscription = DateTime.UtcNow;

            await db.Users.AddAsync(createdUser);
            await db.SaveChangesAsync();

            return new QueryResults<UserDto>
            {
                Data = createdUser.ConvertToDto()
            };
        }

        public async Task<QueryResults<UserDto>> ChangeUser(Guid id, UserDto dto)
        {
            var user = db.Users.GetByIdAsync(id).Result ?? throw new NotFoundApiException();

            user.LastName = dto.LastName;
            user.FirstName = dto.FirstName;
            user.Photo = dto.Photo?.ConvertToModel<User>();
            
            db.Users.Update(user);
            await db.SaveChangesAsync();

            return new QueryResults<UserDto>
            {
                Data = new UserDto { Id = user.Id }
            };
        }

        public async Task DeleteUser(Guid userId)
        {
            var foundUser = db.Users.GetByIdAsync(userId).Result ?? throw new NotFoundApiException();
            db.Users.Remove(foundUser);
            await db.SaveChangesAsync();
        }

        public async Task<QueryResults<UserDto>> Photograph(Guid id, PhotoDto dto)
        {
            var user = await db.Users.Include(u => u.Wallet)
                                     .GetByIdAsync(id) ?? throw new NotFoundApiException();
            user.Photo = dto.ConvertToModel<User>();
            
            await db.SaveChangesAsync();
            return new QueryResults<UserDto>
            {
                Data = user.ConvertToDto()
            };
        }
    }
}
