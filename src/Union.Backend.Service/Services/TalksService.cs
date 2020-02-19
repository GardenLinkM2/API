using System;
using System.Collections.Generic;
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

        public async Task<QueryResults<TalkDto>> GetTalk(Guid UserId)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<List<TalkDto>>> GetAllTalks(Guid userId)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<TalkDto>> AddTalk(TalkDto Talk)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<TalkDto>> AddMessage(MessageDto Message, Guid TalkId)
        {
            throw new WorkInProgressApiException();
        }
        public async Task DeleteTalk(Guid TalkId)
        {
            throw new WorkInProgressApiException();
        }
    }
}
