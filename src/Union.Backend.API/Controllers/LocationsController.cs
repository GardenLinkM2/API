using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Model.Models;
using Union.Backend.Service.Services;
using Union.Backend.Service.Results;
using System;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Dtos;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly LocationsService service;
        public LocationsController(LocationsService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<LocationsQueryResults> GetAllLocations()
        {
            return await service.GetAllLocations();
        }

        [HttpGet("{id}")]
        public async Task<LocationQueryResults> GetLocationById([FromRoute(Name = "id")] Guid LocationId)
        {
            return await service.GetLocation(LocationId);
        }

        [HttpPost]
        public async Task<LocationQueryResults> CreateLocation([FromBody] LocationDto Location)
        {
            return await service.AddLocation(Location);
        }

        [HttpPut("{id}")]
        public async Task<LocationQueryResults> AddMessage([FromRoute(Name = "id")] Guid LocationId, [FromBody] LocationDto Location)
        {
            return await service.ChangeLocation(Location, LocationId);
        }



        [HttpDelete("{id}")]
        public async Task DeleteLocation([FromRoute(Name = "id")] Guid LocationId)
        {
            await service.DeleteLocation(LocationId);
        }

    }
}
