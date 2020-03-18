using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Results;

namespace Union.Backend.Service.Services
{
    public class TalksService
    {
        private readonly GardenLinkContext db;
        public TalksService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<QueryResults<List<TalkDto>>> GetMyTalks(Guid me)
        {
            var talks = db.Talks
                .Include(t => t.Messages)
                .Include(t => t.Sender)
                .Include(t => t.Receiver)
                .Where(t => (t.Sender.Id.Equals(me) || t.Receiver.Id.Equals(me)) && !t.IsArchived)
                .Select(t => t.ConvertToDto());

            return new QueryResults<List<TalkDto>>
            {
                Data = await talks.ToListAsync(),
                Count = await talks.CountAsync()
            };
        }

        public async Task<QueryResults<TalkDto>> GetTalkById(Guid me, Guid talkId)
        {
            var talk = await db.Talks
                .Include(t => t.Messages)
                .Include(t => t.Sender)
                .Include(t => t.Receiver)
                .GetByIdAsync(talkId) ?? throw new NotFoundApiException();

            if (talk.Sender.Id != me && talk.Receiver.Id != me)
                throw new ForbiddenApiException();

            return new QueryResults<TalkDto>
            {
                Data = talk.ConvertToDto()
            };
        }

        public async Task<QueryResults<TalkDto>> CreateTalk(Guid me, TalkDto talkDto)
        {
            var userMe = await db.Users.GetByIdAsync(me) ?? throw new NotFoundApiException();
            var receiver = await db.Users.GetByIdAsync(talkDto.Receiver) ?? throw new NotFoundApiException();
            var talk = talkDto.ConvertToModel(userMe, receiver);

            db.Talks.Add(talk);
            await db.SaveChangesAsync();

            return new QueryResults<TalkDto>
            {
                Data = talk.ConvertToDto()
            };
        }

        public async Task<QueryResults<MessageDto>> PostMessageToTalk(Guid me, Guid talkId, MessageDto messageDto)
        {
            var talk = await db.Talks
                .Include(t => t.Messages)
                .Include(t => t.Sender)
                .Include(t => t.Receiver)
                .GetByIdAsync(talkId) ?? throw new NotFoundApiException();

            if (talk.Sender.Id != me && talk.Receiver.Id != me)
                throw new ForbiddenApiException();

            messageDto.Sender = me;
            messageDto.IsRead = false;
            var message = messageDto.ConvertToModel();

            talk.Messages.Add(message);

            await db.SaveChangesAsync();
            return new QueryResults<MessageDto>
            {
                Data = message.ConvertToDto()
            };
        }

        public async Task DeleteTalk(Guid me, Guid talkId)
        {
            var talk = await db.Talks
                .Include(t => t.Sender)
                .GetByIdAsync(talkId) ?? throw new NotFoundApiException();

            if (talk.Sender.Id != me)
                throw new ForbiddenApiException();

            db.Talks.Remove(talk);
            await db.SaveChangesAsync();
        }
    }
}
