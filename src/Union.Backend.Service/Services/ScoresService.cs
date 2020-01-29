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
    public class ScoresService
    {
        private readonly GardenLinkContext db;
        public ScoresService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }


        public async Task<ScoreQueryResults> GetScore(Guid Id)
        {
            throw new NotImplementedException();
        }



        public async Task<ScoreQueryResults> AddScore(ScoreDto Score, Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ScoreQueryResults> ReportScore(Guid id, ScoreDto Score)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteScore(Guid ScoreId)
        {
            throw new NotImplementedException();
        }
    }
}
