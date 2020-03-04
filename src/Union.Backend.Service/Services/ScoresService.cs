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
    public class ScoresService
    {
        private readonly GardenLinkContext db;
        public ScoresService(GardenLinkContext gardenLinkContext)
        {
            db = gardenLinkContext;
        }

        public async Task<QueryResults<ScoreDto>> GetScoreById(Guid scoreId)
        {
            var score = await db.Scores
                .GetByIdAsync(scoreId) ?? throw new NotFoundApiException();

            return new QueryResults<ScoreDto>
            {
                Data = score.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<ScoreDto>>> GetScoresByUserRater(Guid userId)
        {
            var scores = db.Scores
                .Where(s => s.Rater == userId)
                .Select(s => s.ConvertToDto());

            return new QueryResults<List<ScoreDto>>
            {
                Data = await scores.ToListAsync(),
                Count = await scores.CountAsync()
            };
        }

        public async Task<QueryResults<List<ScoreDto>>> GetScoresByUserRated(Guid userId)
        {
            var scores = db.Scores
                .Where(s => s.Rated == userId)
                .Select(s => s.ConvertToDto());

            return new QueryResults<List<ScoreDto>>
            {
                Data = await scores.ToListAsync(),
                Count = await scores.CountAsync()
            };
        }

        public async Task<QueryResults<List<ScoreDto>>> GetScoresByGarden(Guid gardenId)
        {
            var scores = db.Scores
                .Where(s => s.Rated == gardenId)
                .Select(s => s.ConvertToDto());

            return new QueryResults<List<ScoreDto>>
            {
                Data = await scores.ToListAsync(),
                Count = await scores.CountAsync()
            };
        }

        public async Task<QueryResults<ScoreDto>> AddScore(ScoreDto scoreDto, Guid ratedId)
        {
            var createdScore = scoreDto.ConvertToModel();
            createdScore.Rated = ratedId;
            createdScore.Id = new Guid();

            await db.Scores.AddAsync(createdScore);
            await db.SaveChangesAsync();
            return new QueryResults<ScoreDto>
            {
                Data = createdScore.ConvertToDto()
            };
        }

        public async Task<QueryResults<ScoreDto>> ReportScore(Guid id, ScoreDto score)
        {
            throw new WorkInProgressApiException();
        }

        public async Task DeleteScore(Guid scoreId)
        {
            var foundScore = db.Scores.GetByIdAsync(scoreId).Result ?? throw new NotFoundApiException();
            db.Scores.Remove(foundScore);
            await db.SaveChangesAsync();
        }
    }
}
