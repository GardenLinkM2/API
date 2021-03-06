﻿using Microsoft.AspNet.OData.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Union.Backend.Model;
using Union.Backend.Model.DAO;
using Union.Backend.Model.Models;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Results;
using static Union.Backend.Model.Models.ModelExtensions;
using static Union.Backend.Service.Utils;

namespace Union.Backend.Service.Services
{
    public class GardensService
    {
        private readonly GardenLinkContext db;
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

        public async Task<QueryResults<List<GardenDto>>> SearchGardens(ODataQueryOptions<Garden> options, 
                                                                       double? longi, 
                                                                       double? lati, 
                                                                       int? dist)
        {
            var queryable = options.ApplyTo(db.Gardens.Where(g => g.Validation.Equals(Status.Accepted)))
                                   .OfType<Garden>();

            IQueryable<Garden> gardens = queryable.Include(g => g.Photos)
                                                  .Include(g => g.Criteria)
                                                  .Include(g => g.Leasings)
                                                  .Include(g => g.Location)
                                                  .Include(g => g.Owner);

            if (longi.HasValue && lati.HasValue && dist.HasValue)
            {
                var coord = (longi.Value, lati.Value);
                gardens = gardens.Where(g => g.Location.Coordinates.ComputeDistance(coord) <= dist.Value);
            }

            var noneReserved = (await gardens?.ToListAsync()).Where(g => !g.IsReserved);

            return new QueryResults<List<GardenDto>>
            {
                Data = noneReserved?.Select(g => g.ConvertToDto()).ToList(),
                Count = noneReserved?.Count()
            };
        }

        public async Task<QueryResults<List<GardenDto>>> GetGardensByUserId(Guid myId)
        {
            var gardens = db.Gardens.Include(g => g.Photos)
                                    .Include(g => g.Location)
                                    .Include(g => g.Criteria)
                                    .Include(g => g.Owner)
                                    .Where(g => g.Owner.Id.Equals(myId))
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
                                         .Include(g => g.Criteria)
                                         .Include(g => g.Location)
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
            dto.Validation = Status.Accepted;
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
            var garden = db.Gardens.Include(g => g.Photos)
                                   .Include(g => g.Location)
                                   .Include(g => g.Criteria)
                                   .Include(g => g.Owner)
                                   .GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();
            if (!garden.Owner.Id.Equals(me))
                throw new ForbiddenApiException();

            garden.Name = dto.Name ?? garden.Name;
            garden.MinUse = dto.MinUse ?? garden.MinUse;
            garden.Description = dto.Description ?? garden.Description;
            garden.Location = dto.Location?.ConvertToModel() ?? garden.Location;
            garden.Criteria = dto.Criteria?.ConvertToModel() ?? garden.Criteria;
            garden.Photos = dto.Photos?.ConvertToModel<Garden>() ?? garden.Photos;

            db.Gardens.Update(garden);
            await db.SaveChangesAsync();
            return new QueryResults<GardenDto>
            {
                Data = garden.ConvertToDto()
            };
        }

        public async Task<QueryResults<GardenDto>> ChangeGardenValidation(Guid gardenId, ValidationDto val)
        {
            var garden = await db.Gardens.Include(g => g.Photos)
                                         .Include(g => g.Location)
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

        public async Task<QueryResults<ReportDto>> ReportGarden(Guid reporter, Guid gardenId, ReportDto dto)
        {
            _ = await db.Users.GetByIdAsync(reporter) ?? throw new NotFoundApiException();
            var garden = await db.Gardens.GetByIdAsync(gardenId) ?? throw new NotFoundApiException();

            var report = dto.ConvertToModel();
            report.Reporter = reporter;

            garden.Reports = garden.Reports ?? new List<Report>();
            garden.Reports.Add(report);

            db.Gardens.Update(garden);
            await db.SaveChangesAsync();

            return new QueryResults<ReportDto>
            {
                Data = report.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<GardenDto>>> GetReportedGardens()
        {
            var gardens = db.Gardens.Include(g => g.Photos)
                                    .Include(g => g.Location)
                                    .Include(g => g.Criteria)
                                    .Include(g => g.Owner)
                                    .Include(g => g.Reports)
                                    .Where(g => g.Reports.Any())
                                    .Select(g => g.ConvertToDto());

            return new QueryResults<List<GardenDto>>
            {
                Data = await gardens.ToListAsync(),
                Count = await gardens.CountAsync()
            };
        }

        public async Task DeleteGarden(Guid gardenId)
        {
            var garden = db.Gardens.Include(g => g.Leasings)
                                   .GetByIdAsync(gardenId).Result ?? throw new NotFoundApiException();

            if (garden.Leasings?.Any(l => l.State.Equals(LeasingStatus.InProgress)) ?? false)
                throw new BadRequestApiException("Impossible to delete a garden with current leasings.");

            db.Gardens.Remove(garden);
            db.Leasings.RemoveRange(garden.Leasings.Where(l => l.State.Equals(LeasingStatus.InDemand)));

            await db.SaveChangesAsync();
        }

        public (double longitude, double latitude) GetCoordinates(NullableLocationDto location)
        {
            if (!string.IsNullOrEmpty(location.Street) && location.StreetNumber.HasValue && location.PostalCode.HasValue)
            {
                var queryString = $"?q={ location.StreetNumber }+{ Regex.Replace(location.Street.Trim(), @"\s+", "+") }+&postcode={ location.PostalCode }";
                return GetCoordinatesFromUrl(AppSettings.GEOCAL_API_URL + queryString);
            }
            else if (!string.IsNullOrEmpty(location.City) && location.PostalCode.HasValue)
            {
                string queryString = $"?q={location.City.Trim()}&postcode={location.PostalCode}";
                return GetCoordinatesFromUrl(AppSettings.GEOCAL_API_URL + queryString);
            }
            else
                throw new BadRequestApiException($"Not enough fields completed in the {nameof(NullableLocationDto)} object.");
        }
    }
}
