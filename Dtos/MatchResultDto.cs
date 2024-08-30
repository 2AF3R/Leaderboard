using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Dtos
{
    public class MatchResultDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Points must be a non-negative integer.")]
        public int Points { get; set; }
    }
}
