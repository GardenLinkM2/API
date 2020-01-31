using System;
using System.Linq;
using Union.Backend.Model.Models;
using Union.Backend.Service.Dtos;

namespace Union.Backend.Service.Services
{
    static class Extensions
    {
        public static User ConvertToModel(this UserDto dto)
        {
            return new User
            {
                Name = dto.Name
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
                Photos = user.Photos?.Select(p => p.ConvertToDto()).ToList(),
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
