using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Union.Backend.Service.Auth;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Services;

namespace Union.Backend.API.Controllers
{
    [Route("api/syn")]
    [ApiController]
    public class ClientDialogController : ControllerBase
    {
        private readonly ClientDialogService service;

        public ClientDialogController(ClientDialogService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Authorize(PermissionType.All)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TokenDto))]
        public async Task<IActionResult> Syn([FromBody] TokenDto tokenDto)
        {
            return Ok(await service.Syn(tokenDto));
        }
    }
}
