namespace HackerNews.Api.Services
{
    public interface ICacheService
    {
        bool TryRead(int id, out string result);
        void Set(int id, string content, int expirationHours);
    }
}