using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using System;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Auth;
using System.Net;

namespace Union.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactsService service;
        public ContactsController(ContactsService service)
        {
            this.service = service;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Contact([FromRoute(Name = "id")] Guid contactId, [FromBody] DemandDto demand)
        {
            try
            {
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                //await service.Contact(myId, contactId, contact);
                await service.Contact(contactId, myId, demand); //TEMP
                return NoContent();
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

        [HttpGet("me")]
        public async Task<IActionResult> GetMyContacts()
        {
            try
            {
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetMyContacts(myId));
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

        [HttpPut("{id}")]
        public async Task<IActionResult> AcceptOrDenyContact([FromRoute(Name = "id")] Guid demandId, [FromBody] DemandDto demand)
        {
            try
            {
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.AcceptOrDenyContact(myId, demandId, demand));
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

        [HttpDelete("{id}")]
        public async Task DeleteContact([FromRoute(Name = "id")] Guid userId)
        {
            await service.DeleteContact(userId);
        }
    }
}
