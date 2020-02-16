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
                Photo = user.Photo?.ConvertToDto(),
                Wallet = user.Wallet?.ConvertToDto()
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
                FileName = photo.FileName
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

        public static GardenDto ConvertToDto(this Garden garden)
        {
            return new GardenDto
            {
                Id = garden.Id,
                Name = garden.Name,
                Size = garden.Size,
                Reserve = garden.Reserve,
                Type = garden.Type,
                MinUse = garden.MinUse,
                Owner = garden.Owner.ConvertToDto(),
                Criteria = garden.Criteria.ConvertToDto(),
                Validation = garden.Validation.ConvertToDto(),
                Photos = garden.Photos?.Select(p => p.ConvertToDto()).ToListIfNotEmpty()
            };
        }

        public static ValidationDto ConvertToDto(this Validation validation)
        {
            return new ValidationDto
            {
                Id = validation.Id,
                State = validation.State
            };
        }

        public static CriteriaDto ConvertToDto(this Criteria criteria)
        {
            return new CriteriaDto
            {
                Id = criteria.Id,
                Area = criteria.Area,
                DirectAccess = criteria.DirectAccess,
                Equipments = criteria.Equipments,
                Orientation = criteria.Orientation,
                Price = criteria.Price,
                TypeOfClay = criteria.TypeOfClay,
                WaterAccess = criteria.WaterAccess,
                LocationTime = criteria.LocationTime,
                Location = criteria.Location.ConvertToDto()
            };
        }

        public static LocationDto ConvertToDto(this Location location)
        {
            return new LocationDto
            {
                Id = location.Id
            };
        }

        public static PaymentDto ConvertToDto(this Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                Collector = payment.Collector.ConvertToDto(),
                Payer = payment.Payer.ConvertToDto(),
                Sum = payment.Sum,
                State = payment.State,
                Leasing = payment.Leasing.ConvertToDto()
            };
        }

        public static LeasingDto ConvertToDto(this Leasing leasing)
        {
            return new LeasingDto
            {
                Id = leasing.Id,
                Begin = leasing.Begin,
                State = leasing.State,
                End = leasing.End,
                Garden = leasing.Garden.ConvertToDto(),
                Owner = leasing.Owner.ConvertToDto(),
                Price = leasing.Price,
                Renew = leasing.Renew,
                Renter = leasing.Renter.ConvertToDto(),
                Time = leasing.Time
            };
        }

        public static TalkDto ConvertToDto(this Talk talk)
        {
            return new TalkDto
            {
                Id = talk.Id,
                Archive = talk.Archive,
                Receiver = talk.Receiver.ConvertToDto(),
                Sender = talk.Sender.ConvertToDto(),
                Subject = talk.Subject
            };
        }

        public static ScoreDto ConvertToDto(this UserNotation notation)
        {
            return new ScoreDto
            {
                Id = notation.Id,
                Comment = notation.Comment,
                Note = notation.Note,
                Rater = notation.Rater.ConvertToDto(),
                Rated = notation.Rated.Id
            };
        }

        public static ContactDto ConvertToDto(this Contact contact)
        {
            return new ContactDto
            {
                Id = contact.Id,
                Contact = contact.MyContact.ConvertToDto(),
                Status = contact.Status,
                FirstMessage = contact.FirstMessage
            };
        }

        public static MessageDto ConvertToDto(this Message message)
        {
            return new MessageDto
            {
                Id = message.Id,
                Date = message.Date,
                Conversation = message.Talk.ConvertToDto(),
                Sender = message.Sender.ConvertToDto(),
                Read = message.Read,
                Text = message.Text,
                Photos = message.Photos?.Select(p => p.ConvertToDto()).ToListIfNotEmpty()
            };
        }
    }
}
