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

        public async Task Contact(Guid me, Guid contact, DemandDto demand)
        {
            var userMe = await db.Users.GetByIdAsync(me) ?? throw new NotFoundApiException();
            if (!db.Users.Any(c => c.Id.Equals(contact)))
                throw new NotFoundApiException();

            db.Contacts.Add(new Contact
            {
                Me = contact,
                MyContact = userMe,
                FirstMessage = demand.FirstMessage,
                Status = ContactStatus.Pending
            });
            await db.SaveChangesAsync();
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

        public async Task<QueryResults<ContactDto>> AcceptOrDenyContact(Guid me, Guid demandId, DemandDto demand)
        {
            var contact = await db
                .Contacts
                .Include(c => c.MyContact)
                .GetByIdAsync(demandId) ?? throw new NotFoundApiException();

            if (!contact.Me.Equals(me) || contact.Status != ContactStatus.Pending)
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