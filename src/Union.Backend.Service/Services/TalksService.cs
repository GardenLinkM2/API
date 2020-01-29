using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Model.Models;
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

        public async Task<TalkQueryResults> GetTalk(Guid UserId)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<TalksQueryResults> GetAllTalks()
        {
            throw new WorkInProgressApiException();
        }

        public async Task<TalkQueryResults> AddTalk(TalkDto Talk)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<TalkQueryResults> AddMessage(MessageDto Message, Guid TalkId)
        {
            throw new WorkInProgressApiException();
        }
        public async Task DeleteTalk(Guid TalkId)
        {
            throw new WorkInProgressApiException();
        }
    }
}
