using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Union.Backend.Service.Auth;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Services;

namespace Union.Backend.API.Controllers
{
    [Route("api/hello")] //TODO: def endpoint
    [ApiController]
    public class ClientDialogController : ControllerBase
    {
        private ClientDialogService service;

        public ClientDialogController(ClientDialogService service)
        {
            this.service = service;
        }

        [HttpPost("syn")]
        [Authorize(PermissionType.All)]
        public async Task<IActionResult> Syn([FromBody] TokenDto tokenDto)
        {
            return Ok(await service.Syn(tokenDto));
        }
    }
}
