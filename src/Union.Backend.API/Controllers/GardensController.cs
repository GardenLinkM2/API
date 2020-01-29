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
    public class GardensController : ControllerBase
    {
        private readonly GardensService service;
        public GardensController(GardensService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<GardensQueryResults> GetAllGardens()
        {
            return await service.GetAllGardens();
        }

        [HttpGet("search/{params}")] //TODO
        public async Task<GardensQueryResults> GetGardenByParam([FromRoute(Name = "id")] Guid GardenId)
        {
            return await service.GetGardenByParams();
        }

        [HttpGet("{id}")]
        public async Task<GardenQueryResults> GetGardenById([FromRoute(Name = "id")] Guid GardenId)
        {
            return await service.GetGardenById(GardenId);
        }

        [HttpPost]
        public async Task<GardenQueryResults> CreateGarden([FromBody] GardenDto Garden)
        {
            return await service.AddGarden(Garden);
        }

        [HttpPut("{id}")]
        public async Task<GardenQueryResults> AddMessage([FromRoute(Name = "id")] Guid GardenId, [FromBody] GardenDto garden)
        {
            return await service.ChangeGarden(garden, GardenId);
        }

        [HttpPut("{id}/description")]
        public async Task<GardenQueryResults> UpdateGardenDescription([FromRoute(Name = "id")] Guid GardenId, [FromBody] DescriptionDto desc)
        {
            return await service.ChangeGardenDescription(desc, GardenId);
        }

        [HttpPut("{id}/validation")]
        public async Task<GardenQueryResults> UpdateGardenValidation([FromRoute(Name = "id")] Guid GardenId, [FromBody] ValidationDto valid)
        {
            return await service.ChangeGardenValidation(valid, GardenId);
        }

        [HttpDelete("{id}")]
        public async Task DeleteGarden([FromRoute(Name = "id")] Guid GardenId)
        {
            await service.DeleteGarden(GardenId);
        }

    }
}
