using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using System;
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
        public async Task<IActionResult> GetAllLocations()
        {
            return Ok(await service.GetAllLocations());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById([FromRoute(Name = "id")] Guid LocationId)
        {
            return Ok(await service.GetLocation(LocationId));
        }

        [HttpPost] //TODO
        public async Task<IActionResult> CreateLocation([FromBody] LeasingDto Location)
        {
            return Created("TODO", await service.AddLocation(Location));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddMessage([FromRoute(Name = "id")] Guid LocationId, [FromBody] LeasingDto Location)
        {
            return Ok(await service.ChangeLocation(Location, LocationId));
        }

        [HttpDelete("{id}")]
        public async Task DeleteLocation([FromRoute(Name = "id")] Guid LocationId)
        {
            await service.DeleteLocation(LocationId);
        }

    }
}
