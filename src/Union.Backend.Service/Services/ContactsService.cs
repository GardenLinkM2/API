using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Model.Models;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Results;

namespace Union.Backend.Service.Services
{
    public class ContactsService
    {
        private readonly GardenLinkContext db;
        public ContactsService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<QueryResults<ContactDto>> Contact(Guid me, Guid contactId, DemandDto demand)
        {
            _ = await db.Users.GetByIdAsync(me) ?? throw new NotFoundApiException();
            var contact = await db.Users.GetByIdAsync(contactId) ?? throw new NotFoundApiException();

            var newContact = new Contact
            {
                Me = me,
                MyContact = contact,
                FirstMessage = demand.FirstMessage,
                Status = Status.Pending
            };
            db.Contacts.Add(newContact);
            await db.SaveChangesAsync();

            return new QueryResults<ContactDto>
            {
                Data = newContact.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<ContactDto>>> GetMyContacts(Guid me)
        {
            var contacts = db.Contacts
                .Where(c => c.Me.Equals(me))
                .Include(c => c.MyContact)
                .Select(c => c.ConvertToDto());
            return new QueryResults<List<ContactDto>>
            {
                Data = await contacts.ToListAsync(),
                Count = await contacts.CountAsync()
            };
        }

        public async Task<QueryResults<ContactDto>> GetContactbyId(Guid me, Guid demandId)
        {
            var contact = await db.Contacts
                .Include(c => c.MyContact)
                .GetByIdAsync(demandId) ?? throw new NotFoundApiException();

            if (contact.Me != me)
                throw new UnauthorizeApiException();

            return new QueryResults<ContactDto>
            {
                Data = contact.ConvertToDto()
            };
        }

        public async Task<QueryResults<ContactDto>> AcceptOrDenyContact(Guid me, Guid demandId, DemandDto demand)
        {
            var contact = await db
                .Contacts
                .Include(c => c.MyContact)
                .GetByIdAsync(demandId) ?? throw new NotFoundApiException();

            if (!contact.Me.Equals(me) || contact.Status != Status.Pending)
                throw new UnauthorizeApiException();

            contact.Status = demand.Status;
            await db.SaveChangesAsync();
            return new QueryResults<ContactDto>
            {
                Data = contact.ConvertToDto()
            };
        }

        public async Task DeleteContact(Guid me, Guid friendId)
        {
            var contact = await db.Contacts.FirstOrDefaultAsync(c => c.Me.Equals(me) && c.MyContact.Equals(friendId)) ?? throw new NotFoundApiException();

            db.Contacts.Remove(contact);
            await db.SaveChangesAsync();
        }
    }
}