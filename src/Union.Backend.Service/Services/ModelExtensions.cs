using System;
using System.Collections.Generic;
using System.Linq;
using Union.Backend.Model.DAO;
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
                Name = dto.Name,
                FirstName = dto.FirstName,
                Mail = dto.Mail,
                PhoneNumber = dto.PhoneNumber
            };
        }
        public static UserDto ConvertToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                FirstName = user.FirstName,
                Mail = user.Mail,
                PhoneNumber = user.PhoneNumber,
                Photos = user.Photos?.Select(p => p.ConvertToDto()).ToListIfNotEmpty(),
                Wallet = user.Wallet.ConvertToDto()
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

        public static WalletDto ConvertToDto(this Wallet wallet)
        {
            return new WalletDto
            {
                Id = wallet.Id,
                Balance = wallet.Balance
            };
        }
    }
}
