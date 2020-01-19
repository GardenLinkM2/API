using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Model.Models;
using Union.Backend.Service.IServices;
using Union.Backend.Service.Results;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, IUsersService
    {
        private readonly IUsersService service;
        public UsersController(IUsersService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<UsersQueryResults> All()
        {
            return await service.All();
        }

        [HttpPost]
        public async Task<UsersQueryResults> Add([FromBody] User user)
        {
            return await service.Add(user);
        }
    }
}
