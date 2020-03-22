using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Union.Backend.Model;
using Union.Backend.Model.Models;
using Union.Backend.Service.Dtos;
using static Union.Backend.Model.Models.ModelExtensions;
using static Union.Backend.Service.Utils;

namespace Union.Backend.Service.Services
{
    static class ConversionExtensions
    {
        public static List<T> ToListIfNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Count() == 0 ? null : enumerable.ToList();
        }

        public static User ConvertToModel(this UserDto dto)
        {
            return new User
            {
                Id = dto.Id, //Necessary
                Mail = dto.Email,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                Photo = dto.Photo?.ConvertToModel<User>()
            };
        }

        public static UserDto ConvertToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Mail,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Photo = user.Photo?.ConvertToDto(),
                Wallet = user.Wallet?.ConvertToDto()
            };
        }

        public static Photo<T> ConvertToModel<T>(this PhotoDto dto)
            where T : IPhotographable
        {
            return new Photo<T>
            {
                FileName = dto.FileName,
                Path = dto.Path
            };
        }
        
        public static List<Photo<T>> ConvertToModel<T>(this ICollection<PhotoDto> dtos)
            where T : IPhotographable
        {
            List<Photo<T>> photos = new List<Photo<T>>();

            foreach (PhotoDto dto in dtos)
            {
                photos.Add(dto.ConvertToModel<T>());
            }

            return photos;
        }

        public static PhotoDto ConvertToDto<T>(this Photo<T> photo)
            where T : IPhotographable
        {
            return new PhotoDto
            {
                FileName = photo.FileName,
                Path = photo.Path
            };
        }

        public static WalletDto ConvertToDto(this Wallet wallet)
        {
            return new WalletDto
            {
                Id = wallet.Id,
                Balance = wallet.RealTimeBalance
            };
        }

        public static GardenDto ConvertToDto(this Garden garden)
        {
            return new GardenDto
            {
                Id = garden.Id,
                Name = garden.Name,
                IsReserved = garden.IsReserved,
                MinUse = garden.MinUse,
                Description = garden.Description,
                Location = garden.Location?.ConvertToDto(),
                Owner = garden.Owner.Id,
                Criteria = garden.Criteria?.ConvertToDto(),
                Validation = garden.Validation,
                Photos = garden.Photos?.Select(p => p.ConvertToDto()).ToListIfNotEmpty(),
                Reports = garden.Reports?.Select(r => r.ConvertToDto()).ToListIfNotEmpty()
            };
        }

        public static Garden ConvertToModel(this GardenDto dto)
        {
            return new Garden
            {
                Name = dto.Name,
                MinUse = dto.MinUse ?? 1,
                Description = dto.Description,
                Location = dto.Location?.ConvertToModel(),
                Criteria = dto.Criteria?.ConvertToModel(),
                Validation = dto.Validation,
                Photos = dto.Photos?.ConvertToModel<Garden>()
            };
        }

        public static CriteriaDto ConvertToDto(this Criteria criteria)
        {
            return new CriteriaDto
            {
                Area = criteria.Area,
                DirectAccess = criteria.DirectAccess,
                Equipments = criteria.Equipments,
                Orientation = criteria.Orientation,
                Price = criteria.Price,
                TypeOfClay = criteria.TypeOfClay,
                WaterAccess = criteria.WaterAccess,
                LocationTime = criteria.LocationTime,
            };
        }

        public static Criteria ConvertToModel(this CriteriaDto dto)
        {
            return new Criteria
            {
                Area = dto.Area,
                DirectAccess = dto.DirectAccess,
                Equipments = dto.Equipments,
                Orientation = dto.Orientation,
                Price = dto.Price,
                TypeOfClay = dto.TypeOfClay,
                WaterAccess = dto.WaterAccess,
                LocationTime = dto.LocationTime
            };
        }

        public static LocationDto ConvertToDto(this Location location)
        {
            return new LocationDto
            {
                StreetNumber = location.StreetNumber,
                Street = location.Street,
                PostalCode = location.PostalCode,
                City = location.City,
                LongitudeAndLatitude = location.Coordinates.ConvertToCoordinates()
            };
        }

        public static Coordinates ConvertToCoordinates(this (double longitude, double latitude) tuple)
        {
            return new Coordinates
            {
                Longitude = tuple.longitude,
                Latitude = tuple.latitude
            };
        }

        public static Location ConvertToModel(this LocationDto dto)
        {
            return new Location
            {
                StreetNumber = dto.StreetNumber,
                Street = dto.Street,
                PostalCode = dto.PostalCode,
                City = dto.City,
                Coordinates = dto.ConvertToCoordinates()
            };
        }

        public static (double longitude, double latitude) ConvertToCoordinates(this LocationDto dto)
        {
            var queryString = $"?q={ dto.StreetNumber }+{ Regex.Replace(dto.Street.Trim(), @"\s+", "+") }+&postcode={ dto.PostalCode }";
            return GetCoordinatesFromUrl(AppSettings.GEOCAL_API_URL + queryString);
        }

        public static Payment ConvertToModel(this PaymentDto dto)
        {
            return new Payment
            {
                Sum = dto.Sum,
                Date = dto.Date.ToDateTime()
            };
        }

        public static PaymentDto ConvertToDto(this Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                Sum = payment.Sum,
                Date = payment.Date.ToTimestamp(),
                Leasing = payment.OfLeasing
            };
        }

        public static LeasingDto ConvertToDto(this Leasing leasing)
        {
            return new LeasingDto
            {
                Id = leasing.Id,
                Creation = leasing.Creation.ToTimestamp(),
                Begin = leasing.Begin.ToTimestamp(),
                End = leasing.End.ToTimestamp(),
                State = leasing.State,
                Renew = leasing.Renew,
                Time = leasing.Time.ToSeconds(),
                Garden = leasing.Garden.Id,
                Renter = leasing.Renter.Id,
                Owner = leasing.Garden.Owner.Id
            };
        }

        public static Leasing ConvertToModel(this LeasingDto dto)
        {
            return new Leasing
            {
                Creation = dto.Creation.ToDateTime(),
                Begin = dto.Begin?.ToDateTime() ?? DateTime.UtcNow,
                End = dto.End?.ToDateTime() ?? DateTime.UtcNow.AddYears(1),
                Renew = dto.Renew.Value,
                State = dto.State.Value,
            };
        }

        public static TalkDto ConvertToDto(this Talk talk)
        {
            return new TalkDto
            {
                Id = talk.Id,
                IsArchived = talk.IsArchived,
                Receiver = talk.Receiver.Id,
                Sender = talk.Sender.Id,
                Subject = talk.Subject,
                Messages = talk.Messages?.Select(m => m.ConvertToDto()).ToList()
            };
        }

        public static Talk ConvertToModel(this TalkDto dto, User sender, User receiver)
        {
            return new Talk
            {
                Subject = dto.Subject,
                Sender = sender,
                Receiver = receiver,
                Messages= dto.Messages?.Select(m => m.ConvertToModel()).ToList()
            };
        }

        public static MessageDto ConvertToDto(this Message message)
        {
            return new MessageDto
            {
                Id = message.Id,
                CreationDate = message.CreationDate.ToTimestamp(),
                Sender = message.Sender,
                IsRead = message.IsRead,
                Text = message.Text,
                Photos = message.Photos?.Select(p => p.ConvertToDto()).ToListIfNotEmpty()
            };
        }

        public static Message ConvertToModel(this MessageDto dto)
        {
            return new Message
            {
                Id = dto.Id,
                CreationDate = DateTime.UtcNow,
                Sender = dto.Sender,
                IsRead = dto.IsRead,
                Text = dto.Text,
                Photos = dto.Photos?.Select(p => p.ConvertToModel<Message>()).ToList()
            };
        }

        public static ScoreDto ConvertToDto(this Score score)
        {
            return new ScoreDto
            {
                Id = score.Id,
                Comment = score.Comment,
                Mark = score.Mark,
                Rater = score.Rater.Id,
                Rated = score.Rated,
            };
        }

        public static Score ConvertToModel(this ScoreDto dto)
        {
            return new Score
            {
                Comment = dto.Comment,
                Mark = dto.Mark,
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

        public static ReportDto ConvertToDto(this Report report)
        {
            return new ReportDto
            {
                Id = report.Id,
                Reason = report.Reason,
                Comment = report.Comment,
                OfGarden = report.OfGarden.Id
            };
        }

        public static Report ConvertToModel(this ReportDto dto)
        {
            return new Report
            {
                Reason = dto.Reason,
                Comment = dto.Comment
            };
        }
    }
}
