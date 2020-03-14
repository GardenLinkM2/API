using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Service.Auth;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Services;

namespace Union.Backend.API.Controllers
{
    [Route("admin")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdminManagementController : ControllerBase
    {
        private readonly GardenLinkContext db;
        private readonly GardensService gardensService;
        private readonly LeasingsService leasingsService;
        private readonly PaymentsService paymentsService;
        private readonly ScoresService scoresService;
        private readonly UsersService usersService;
        private readonly WalletsService walletsService;

        public AdminManagementController(GardenLinkContext db,
                                         GardensService gardensService,
                                         LeasingsService leasingsService,
                                         PaymentsService paymentsService,
                                         ScoresService scoresService,
                                         UsersService usersService,
                                         WalletsService walletsService)
        {
            this.db = db;
            this.gardensService = gardensService;
            this.leasingsService = leasingsService;
            this.paymentsService = paymentsService;
            this.scoresService = scoresService;
            this.usersService = usersService;
            this.walletsService = walletsService;
        }

        [HttpGet("bimbamboum")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult BimBamBoum()
        {
            db.Migrate();
            return NoContent();
        }

        [HttpGet("gardens/pendings")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GardenDto>))]
        public async Task<IActionResult> GetPendingGardens()
        {
            return Ok(await gardensService.GetPendingGardens());
        }

        [HttpPut("gardens/{id}/validation")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GardenDto))]
        public async Task<IActionResult> UpdateGardenValidation([FromRoute(Name = "id")] Guid gardenId, [FromBody] ValidationDto valid)
        {
            return Ok(await gardensService.ChangeGardenValidation(gardenId, valid));
        }

        [HttpGet("leasings")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LeasingDto>))]
        public async Task<IActionResult> GetAllLeasings()
        {
            return Ok(await leasingsService.GetAllLeasings());
        }

        [HttpGet("gardens/reported")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GardenDto>))]
        public async Task<IActionResult> GetAllReportedGardens()
        {
            return Ok(await gardensService.GetReportedGardens());
        }

        [HttpGet("leasing/user/{id}")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LeasingDto>))]
        public async Task<IActionResult> GetAllUserLeasings([FromRoute(Name = "id")] Guid userId)
        {
            return Ok(await leasingsService.GetAllLeasingsByUserId(userId));
        }

        [HttpGet("payments")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PaymentDto>))]
        public async Task<IActionResult> GetAllPayments()
        {
            return Ok(await paymentsService.GetAllPayments());
        }

        [HttpGet("payments/{id}/user")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PaymentDto>))]
        public async Task<IActionResult> GetAllPaymentsByUserId([FromRoute(Name = "id")] Guid userId)
        {
            return Ok(await paymentsService.GetMyPayments(userId));
        }

        [HttpDelete("payments/{id}")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePayment([FromRoute(Name = "id")] Guid paymentId)
        {
            await paymentsService.DeletePayment(paymentId);
            return NoContent();
        }

        [HttpGet("scores/reported")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ScoreDto>))]
        public async Task<IActionResult> GetAllReportedScores()
        {
            return Ok(await scoresService.GetReportedScores());
        }

        [HttpPost("users")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        public async Task<IActionResult> AddUser([FromBody] UserDto user)
        {
            var result = await usersService.AddUser(user, Guid.NewGuid());
            return Created($"/api/users/{result.Data.Id}", result);
        }

        [HttpPut("users/{id}")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<IActionResult> ChangeUser([FromRoute(Name = "id")] Guid userId, [FromBody] UserDto user)
        {
            return Ok(await usersService.ChangeUser(userId, user));
        }

        [HttpDelete("users/{id}")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUser([FromRoute(Name = "id")] Guid userId)
        {
            await usersService.DeleteUser(userId);
            return NoContent();
        }

        [HttpGet("wallets/user/{id}")]
        [Authorize(PermissionType.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WalletDto))]
        public async Task<IActionResult> GetWalletByUserId([FromRoute(Name = "id")] Guid UserId)
        {
            try
            {
                return Ok(await walletsService.GetWalletByUserId(UserId));
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new BadRequestApiException();
            }
        }
    }
}
