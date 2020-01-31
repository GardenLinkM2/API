using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using System;
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
        public async Task<IActionResult> GetAllGardens()
        {
            return Ok(await service.GetAllGardens());
        }

        [HttpGet("search/{params}")] //TODO
        public async Task<IActionResult> GetGardenByParam([FromRoute(Name = "id")] Guid GardenId)
        {
            return Ok(await service.GetGardenByParams());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGardenById([FromRoute(Name = "id")] Guid GardenId)
        {
            return Ok(await service.GetGardenById(GardenId));
        }

        [HttpPost] //TODO
        public async Task<IActionResult> CreateGarden([FromBody] GardenDto Garden)
        {
            return Created("TODO", await service.AddGarden(Garden));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddMessage([FromRoute(Name = "id")] Guid GardenId, [FromBody] GardenDto garden)
        {
            return Ok(await service.ChangeGarden(garden, GardenId));
        }

        [HttpPut("{id}/description")]
        public async Task<IActionResult> UpdateGardenDescription([FromRoute(Name = "id")] Guid GardenId, [FromBody] DescriptionDto desc)
        {
            return Ok(await service.ChangeGardenDescription(desc, GardenId));
        }

        [HttpPut("{id}/validation")]
        public async Task<IActionResult> UpdateGardenValidation([FromRoute(Name = "id")] Guid GardenId, [FromBody] ValidationDto valid)
        {
            return Ok(await service.ChangeGardenValidation(valid, GardenId));
        }

        [HttpDelete("{id}")]
        public async Task DeleteGarden([FromRoute(Name = "id")] Guid GardenId)
        {
            await service.DeleteGarden(GardenId);
        }
    }
}
