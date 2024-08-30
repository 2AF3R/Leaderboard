namespace Leaderboard.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string DeviceId { get; set; }
        public int Level { get; set; }
        public int TrophyCount { get; set; }
    }
}
