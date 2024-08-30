using StackExchange.Redis;

namespace Leaderboard.Services
{
    public interface IRedisService
    {
        Task SetValueAsync(string key, string value);
        Task<string> GetValueAsync(string key);
        Task AddToSortedSetAsync(string key, string member, double score);
        Task<SortedSetEntry[]> GetSortedSetRangeByScoreAsync(string key, double start, double stop, Order order = Order.Descending);
        Task<long?> GetRankInSortedSetAsync(string key, string member, Order order = Order.Descending);
        Task UpdateLeaderboardAsync(string leaderboardKey, string userId, int points);
    }
}
