using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Union.Backend.Service.Services;
using System;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Auth;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ContactDto))]

        public async Task<IActionResult> Contact([FromRoute(Name = "id")] Guid contactId, [FromBody] DemandDto demand)
        {
            try
            {
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                var result = await service.Contact(myId, contactId, demand);
                return Created($"/api/contacts/{result.Data.Id}", result);
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ContactDto>))]
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContactDto))]
        public async Task<IActionResult> GetContactbyId([FromRoute(Name = "id")] Guid demandId)
        {
            try
            {
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                return Ok(await service.GetContactbyId(myId, demandId));
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContactDto))]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteContact([FromRoute(Name = "id")] Guid friendId)
        {
            try
            {
                var myId = Utils.ExtractIdFromToken(Request.Headers[HttpRequestHeader.Authorization.ToString()]);
                await service.DeleteContact(myId, friendId);
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
    }
}
