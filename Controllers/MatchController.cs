using Leaderboard.Data;
using Leaderboard.Dtos;
using Leaderboard.Models;
using Leaderboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Leaderboard.Controllers
{
    public class MatchController : Controller
    {
        private readonly LeaderboardContext _context;
        private readonly RedisService _redisService;
        public MatchController(LeaderboardContext context, RedisService redisService)
        {
            _context = context;
            _redisService = redisService;
        }
        [HttpPost("match/result")]
        public async Task<IActionResult> SubmitMatchResult([FromBody] MatchResultDto dto)
        {
            var user = await _context.Players.FindAsync(dto.UserId);
            if (user == null)
            {
                return BadRequest("Invalid user ID.");
            }

            var score = new Score
            {
                PlayerId = dto.UserId,
                Value = dto.Points,
                AchievedAt = DateTime.UtcNow
            };

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            await UpdateLeaderboard(user, score);
            return Ok("Score submitted successfully.");
        }

        private async Task UpdateLeaderboard(Player user, Score score)
        {

            await _redisService.UpdateLeaderboardAsync("global_leaderboard", user.Id, score.Value);
            var leaderboardKey = "leaderboard";

            await _redisService.AddToSortedSetAsync(leaderboardKey, user.Username, score.Value);

            var rank = await _redisService.GetRankInSortedSetAsync(leaderboardKey, user.Username);
            if (rank.HasValue)
            {
                Console.WriteLine($"User {user.Username} is now ranked #{rank + 1}");
            }
        }
    }
}
