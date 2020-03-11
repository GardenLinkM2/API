using Microsoft.AspNet.OData.Query;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Model.Models;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Results;

namespace Union.Backend.Service.Services
{
    public class GardensService
    {
        private readonly GardenLinkContext db;
        public const string GEOCAL_API_URL = "https://api-adresse.data.gouv.fr/search/?q=";
        public GardensService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<QueryResults<List<GardenDto>>> GetPendingGardens()
        {
            var gardens = db.Gardens.Include(g => g.Photos)
                                    .Include(g => g.Criteria)
                                    .Include(g => g.Location)
                                    .Include(g => g.Owner)
                                    .Where(g => g.Validation.Equals(Status.Pending))
                                    .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task<QueryResults<List<GardenDto>>> SearchGardens(ODataQueryOptions<Garden> options)
        {
            var queryable = options.ApplyTo(db.Gardens.Where(g => g.Validation.Equals(Status.Accepted)))
                                   .OfType<Garden>();

            var gardens = queryable.Include(g => g.Photos)
                                   .Include(g => g.Criteria)
                                   .Include(g => g.Location)
                                   .Include(g => g.Owner)
                                   .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task<QueryResults<List<GardenDto>>> GetMyGardens(Guid myId)
        {
            var gardens = db.Gardens.Include(g => g.Photos)
                                    .Include(g => g.Validation)
                                    .Include(g => g.Criteria)
                                    .Include(g => g.Owner)
                                    .Where(g => g.Owner.Equals(myId))
                                    .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task<QueryResults<GardenDto>> GetGardenById(Guid gardenId)
        {
            var garden = await db.Gardens.Include(g => g.Photos)
                                         .Include(g => g.Validation)
                                         .Include(g => g.Criteria)
                                         .Include(g => g.Owner)
                                         .GetByIdAsync(gardenId) ?? throw new NotFoundApiException();

            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> AddGarden(Guid me, GardenDto dto)
        {
            var owner = await db.Users.GetByIdAsync(me) ?? throw new NotFoundApiException();

            dto.IsReserved = false;
            dto.Validation = Status.Pending;
            var garden = dto.ConvertToModel();

            owner.Gardens = owner.Gardens ?? new List<Garden>();
            owner.Gardens.Add(garden);

            await db.SaveChangesAsync();

            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> ChangeGarden(Guid me, Guid gardenId, GardenDto dto)
        {
            var garden = db.Gardens.Include(g => g.Location)
                                   .Include(g => g.Criteria)
                                   .Include(g => g.Owner)
                                   .GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();
            if (!garden.Owner.Equals(me))
                throw new ForbidenApiException();



            garden.Name = dto.Name ?? garden.Name;
            garden.Size = dto.Size ?? garden.Size;
            garden.MinUse = dto.MinUse ?? garden.MinUse;
            garden.Description = dto.Description ?? garden.Description;
            garden.Location = dto.Location?.ConvertToModel() ?? garden.Location;
            garden.Criteria = dto.Criteria?.ConvertToModel() ?? garden.Criteria;

            db.Gardens.Update(garden);
            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> ChangeGardenValidation(Guid gardenId, ValidationDto val)
        {
            var garden = await db.Gardens.Include(g => g.Location)
                                         .Include(g => g.Criteria)
                                         .Include(g => g.Owner)
                                         .GetByIdAsync(gardenId) ?? throw new NotFoundApiException();
            if (!garden.Validation.Equals(Status.Pending))
                throw new UnauthorizeApiException();

            garden.Validation = val.Status;

            db.Gardens.Update(garden);

            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task DeleteGarden(Guid gardenId)
        {
            var garden = db.Gardens.GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();

            db.Gardens.Remove(garden);

            await db.SaveChangesAsync();
        }


        public Tuple<double, double> getCoordinates(NullableLocationDto location)
        {
            Tuple<double, double> coord;
            if (!String.IsNullOrEmpty(location.Street) && location.StreetNumber.HasValue && location.PostalCode.HasValue)
            {

                string rue = location.Street.Trim().Replace("  ", " ").Replace(" ", "+");
                string addresse = location.Street + "+" + rue + "+" + "&postcode=" + location.PostalCode;
                coord = getCoordinatesWithUrl(GEOCAL_API_URL + addresse);
            }
            else if (!String.IsNullOrEmpty(location.City) && location.PostalCode.HasValue)
            {
                String addresse = location.City.Trim() + "&postcode=" + location.PostalCode;
                coord = getCoordinatesWithUrl(GEOCAL_API_URL + addresse);
            }
            else
            {
                throw new BadRequestApiException();
            }

            return coord;
        }


        private Tuple<double, double> getCoordinatesWithUrl(String url)
        {
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string urlText = reader.ReadToEnd(); // it takes the response from your url 
            dynamic result = JObject.Parse(urlText);

            double longitude = result.features[0].geometry.coordinates[0];
            double latitude = result.features[0].geometry.coordinates[1];

            return new Tuple<double, double>(longitude, latitude);
        }


        private int calcDist(Tuple<double, double> locationA, Tuple<double, double> locationB)
        {
            double Alongitude = locationA.Item1;
            double Alatitude = locationA.Item2;

            double Blongitude = locationB.Item1;
            double Blatitude = locationB.Item2;

            double x = (Blongitude - Alongitude) * Math.Cos(((Alatitude + Blatitude) / 2) * (Math.PI / 180.0));
            double y = Blatitude - Alatitude;
            double z = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));


            int distance = Convert.ToInt32(Math.Floor(1.852 * 60 * z));
            return distance;

        }

    }
}
