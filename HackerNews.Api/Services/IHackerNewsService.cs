using System.Collections.Generic;
using System.Threading.Tasks;
using HackerNews.Api.Models;

namespace HackerNews.Api.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<int>> GetBestStoriesIds();
        Task<Story> GetStoryDetail(int id);
    }
}