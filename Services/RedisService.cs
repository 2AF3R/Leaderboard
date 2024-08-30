using StackExchange.Redis;

namespace Leaderboard.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = _redis.GetDatabase();
        }

        public async Task SetValueAsync(string key, string value)
        {
            await _database.StringSetAsync(key, value);
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task AddToSortedSetAsync(string key, string member, double score)
        {
            await _database.SortedSetAddAsync(key, member, score);
        }

        public async Task<SortedSetEntry[]> GetSortedSetRangeByScoreAsync(string key, double start, double stop, Order order = Order.Descending)
        {
            return await _database.SortedSetRangeByScoreWithScoresAsync(key, start, stop, order: order);
        }

        public async Task<long?> GetRankInSortedSetAsync(string key, string member, Order order = Order.Descending)
        {
            return await _database.SortedSetRankAsync(key, member, order);
        }
        public async Task UpdateLeaderboardAsync(string leaderboardKey, string userId, int points)
        {
            // Convert userId to a string because Redis stores strings
            string userIdString = userId.ToString();

            // Add the user with the new score to the sorted set in Redis
            await AddToSortedSetAsync(leaderboardKey, userIdString, points);

            // Optionally, you could retrieve and return the rank or updated leaderboard here
            long? rank = await GetRankInSortedSetAsync(leaderboardKey, userIdString);

            // Log or handle rank as needed
            if (rank.HasValue)
            {
                Console.WriteLine($"User {userId} is now ranked #{rank + 1}");
            }
        }
    }

}
