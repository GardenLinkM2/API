using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Union.Backend.Model.Models;
using Union.Backend.Service.Dtos;

namespace Union.Backend.Service.Services
{
    static class ModelExtensions
    {
        public const string GEOCAL_API_URL = "https://api-adresse.data.gouv.fr/search/?q=";

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
                FirstName = dto.FirstName
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

        public static Photo<T> ConvertToModel<T>(this PhotoDto dto, Guid id)
            where T : IPhotographable
        {
            return new Photo<T>
            {
                FileName = dto.FileName,
                RelatedTo = id
            };
        }

        public static List<Photo<T>> ConvertToModel<T>(this List<PhotoDto> dto, Guid id)
            where T : IPhotographable
        {
            List<Photo<T>> photos = new List<Photo<T>>();

            foreach (PhotoDto p in dto)
            {
                photos.Add(new Photo<T>
                {
                    FileName = p.FileName,
                    RelatedTo = id
                });
            }

            return photos;
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
                IsReserved = garden.IsReserved,
                MinUse = garden.MinUse,
                Description = garden.Description,
                Location = garden.Location?.ConvertToDto(),
                Owner = garden.Owner.Id,
                Criteria = garden.Criteria?.ConvertToDto(),
                Validation = garden.Validation,
                Photos = garden.Photos?.Select(p => p.ConvertToDto()).ToListIfNotEmpty()
            };
        }

        public static Garden ConvertToModel(this GardenDto dto)
        {
            return new Garden
            {
                Name = dto.Name,
                Size = dto.Size ?? 1,
                IsReserved = dto.IsReserved ?? false,
                MinUse = dto.MinUse ?? 1,
                Description = dto.Description,
                Location = dto.Location.ConvertToModel(),
                Criteria = dto.Criteria.ConvertToModel(),
                Validation = dto.Validation
            };
        }

        public static long ToSecond(this TimeSpan timeSpan) =>
            timeSpan.Ticks / 10000000;

        public static TimeSpan ToTimeSpan(this long seconds) =>
            new TimeSpan(seconds * 10000000);

        public static CriteriaDto ConvertToDto(this Criteria criteria)
        {
            return new CriteriaDto
            {
                Area = criteria.Area,
                DirectAccess = criteria.DirectAccess,
                Equipments = criteria.Equipments,
                Orientation = criteria.Orientation.ToString(),
                Price = criteria.Price,
                TypeOfClay = criteria.TypeOfClay,
                WaterAccess = criteria.WaterAccess,
                LocationTime = criteria.LocationTime.ToSecond(),
            };
        }

        public static Criteria ConvertToModel(this CriteriaDto dto)
        {
            return new Criteria
            {
                Area = dto.Area,
                DirectAccess = dto.DirectAccess,
                Equipments = dto.Equipments,
                Orientation = (Orientation)Enum.Parse(typeof(Orientation), dto.Orientation),
                Price = dto.Price,
                TypeOfClay = dto.TypeOfClay,
                WaterAccess = dto.WaterAccess,
                LocationTime = dto.LocationTime.ToTimeSpan()
            };
        }

        public static LocationDto ConvertToDto(this Location location)
        {
            return new LocationDto
            {
                StreetNumber = location.StreetNumber,
                Street = location.Street,
                PostalCode = location.PostalCode,
                City = location.City
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
                Coord= getCoordinates(dto)
            };
        }

        public static Payment ConvertToModel(this PaymentDto dto)
        {
            return new Payment
            {
                Sum = dto.Sum,
                Date = dto.Date
            };
        }

        public static PaymentDto ConvertToDto(this Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                Sum = payment.Sum,
                Date = payment.Date,
                Leasing = payment.OfLeasing
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
                Renew = leasing.Renew,
                Time = leasing.Time.ToSecond(),
                Garden = leasing.Garden.Id,
                Renter = leasing.Renter.Id,
                Owner = leasing.Garden.Owner.Id
            };
        }

        public static Leasing ConvertToModel(this LeasingDto dto)
        {
            return new Leasing
            {
                Begin = dto.Begin.Value,
                End = dto.End.Value,
                Renew = dto.Renew.Value,
                State = dto.State.Value,
                Time = dto.Time.Value.ToTimeSpan()
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
                Receiver = receiver
            };
        }

        public static MessageDto ConvertToDto(this Message message)
        {
            return new MessageDto
            {
                Id = message.Id,
                CreationDate = message.CreationDate,
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
                Text = dto.Text
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
                IsReported = score.IsReported
            };
        }

        public static Score ConvertToModel(this ScoreDto dto)
        {
            return new Score
            {
                Comment = dto.Comment,
                Mark = dto.Mark,
                IsReported = dto.IsReported
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

        public static Tuple<double, double> getCoordinates(LocationDto dto)
        {
            String rue = dto.Street.Trim().Replace("  ", " ").Replace(" ", "+");
            String addresse = dto.Street + "+" + rue + "+" + "&postcode=" + dto.PostalCode;

            WebRequest request = HttpWebRequest.Create(GEOCAL_API_URL + addresse);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string urlText = reader.ReadToEnd(); // it takes the response from your url 
            dynamic result = JObject.Parse(urlText);

            double longitude = result.features[0].geometry.coordinates[0];
            double latitude = result.features[0].geometry.coordinates[1];

            return new Tuple<double, double>(longitude, latitude);
        }

    }
}
