using System;
using System.Collections.Generic;
using System.Linq;
using Union.Backend.Model.Models;
using Union.Backend.Service.Dtos;

namespace Union.Backend.Service.Services
{
    static class ModelExtensions
    {
        public static List<T> ToListIfNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Count() == 0 ? null : enumerable.ToList();
        }

        public static User ConvertToModel(this UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                UserName = dto.UserName,
                LastName = dto.LastName,
                FirstName = dto.FirstName
            };
        }

        public static UserDto ConvertToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Photos = user.Photos?.Select(p => p.ConvertToDto()).ToListIfNotEmpty(),
                Wallet = user.Wallet
            };
        }

        public static Photo<T> ConvertToModel<T>(this PhotoDto dto, Guid id)
            where T : IPhotographable
        {
            return new Photo<T>
            {
                FileName = dto.FileName,
                RelatedTo = id
            };
        }
        public static PhotoDto ConvertToDto<T>(this Photo<T> photo)
            where T : IPhotographable
        {
            return new PhotoDto
            {
                Id = photo.Id,
                FileName = photo.FileName,
                RelatedTo = photo.RelatedTo
            };
        }
    }
}
