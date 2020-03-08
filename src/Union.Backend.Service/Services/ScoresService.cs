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
                                       .GetByIdAsync(scoreId)
                        ?? throw new NotFoundApiException();

            return new QueryResults<ScoreDto>
            {
                Data = score.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<ScoreDto>>> GetUserScores(Guid userId)
        {
            _ = await db.Users.GetByIdAsync(userId) ?? throw new NotFoundApiException();

            var scores = db.Scores
                                  .Where(s => s.Rated == userId)
                                  .Select(s => s.ConvertToDto());

            return new QueryResults<List<ScoreDto>>
            {
                Data = await scores.ToListAsync(),
                Count = await scores.CountAsync()
            };
        }

        public async Task<QueryResults<List<ScoreDto>>> GetGardenScores(Guid gardenId)
        {
            _ = await db.Gardens.GetByIdAsync(gardenId) ?? throw new NotFoundApiException();

            var scores = db.Scores
                                  .Where(s => s.Rated == gardenId)
                                  .Select(s => s.ConvertToDto());

            return new QueryResults<List<ScoreDto>>
            {
                Data = await scores.ToListAsync(),
                Count = await scores.CountAsync()
            };
        }

        public async Task<QueryResults<ScoreDto>> AddScore(Guid myId, Guid ratedId, ScoreDto dto)
        {
            if (!db.Users.Any(u => u.Id.Equals(ratedId)) && !db.Gardens.Any(g => g.Id.Equals(ratedId)))
                throw new NotFoundApiException();

            var me = await db.Users.GetByIdAsync(myId) ?? throw new NotFoundApiException();
            var score = dto.ConvertToModel();
            score.Rated = ratedId;

            me.AsRater = me.AsRater ?? new List<Score>();
            me.AsRater.Add(score);

            await db.SaveChangesAsync();

            return new QueryResults<ScoreDto>
            {
                Data = score.ConvertToDto()
            };
        }

        public async Task<QueryResults<List<ScoreDto>>> GetReportedScores()
        {
            var scores = db.Scores
                                  .Where(s => s.Reported == true)
                                  .Select(s => s.ConvertToDto());

            return new QueryResults<List<ScoreDto>>
            {
                Data = await scores.ToListAsync(),
                Count = await scores.CountAsync()
            };
        }

        public async Task<QueryResults<ScoreDto>> ReportScore(Guid scoreId)
        {
            var score = await db.Scores.GetByIdAsync(scoreId) ?? throw new NotFoundApiException();
            score.Reported = true;

            db.Scores.Update(score);
            await db.SaveChangesAsync();
            return new QueryResults<ScoreDto>
            {
                Data = score.ConvertToDto()
            };
        }

        public async Task DeleteScore(Guid scoreId)
        {
            var foundScore = db.Scores.GetByIdAsync(scoreId).Result ?? throw new NotFoundApiException();
            db.Scores.Remove(foundScore);
            await db.SaveChangesAsync();
        }
    }
}
