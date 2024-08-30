using Leaderboard.Data;
using Leaderboard.Dtos;
using Leaderboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Controllers
{
    public class RegisterController : Controller
    {
        private readonly LeaderboardContext _context;
        public RegisterController(LeaderboardContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Players.AnyAsync(u => u.Username == dto.Username))
            {
                return BadRequest("Username already exists.");
            }

            var user = new Player
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                DeviceId = dto.DeviceId,
                RegistrationDate = DateTime.UtcNow
            };

            _context.Players.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }
    }
}
