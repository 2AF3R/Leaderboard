namespace Leaderboard.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Value { get; set; }
        public DateTime AchievedAt { get; set; }

        public Player Player { get; set; }
    }
}
