using System;
using System.Threading.Tasks;
using Union.Backend.Model.DAO;
using Union.Backend.Service.Dtos;
using Union.Backend.Service.Exceptions;
using Union.Backend.Service.Results;

namespace Union.Backend.Service.Services
{
    public class ScoresService
    {
        private readonly GardenLinkContext db;
        public ScoresService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<QueryResults<ScoreDto>> GetScore(Guid Id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<ScoreDto>> AddScore(ScoreDto Score, Guid Id)
        {
            throw new WorkInProgressApiException();
        }

        public async Task<QueryResults<ScoreDto>> ReportScore(Guid id, ScoreDto Score)
        {
            throw new WorkInProgressApiException();
        }

        public async Task DeleteScore(Guid ScoreId)
        {
            throw new WorkInProgressApiException();
        }
    }
}
