using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackerNews.Domain;
using HackerNews.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IHackerNewsService _service;

        public StoriesController(IHackerNewsService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// Returns the top 20 best stories provided by HackerNews.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = new List<Story>();
            var stories = await _service.GetBestStoriesIds();
            
            Parallel.ForEach(stories, s =>
                result.Add(_service.GetStoryDetail(s).Result));

            return Ok(result
                .OrderByDescending(s => s.Score)
                .Take(20)
                .ToList());
        }
    }
}