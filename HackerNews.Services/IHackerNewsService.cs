using System.Collections.Generic;
using System.Threading.Tasks;
using HackerNews.Domain;

namespace HackerNews.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<int>> GetBestStoriesIds();
        Task<Story> GetStoryDetail(int id);
    }
}