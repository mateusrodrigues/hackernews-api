using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackerNews.Api.Models;
using HackerNews.Api.Services;
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

            foreach (var story in stories.Take(20))
                result.Add(await _service.GetStoryDetail(story));
            
            return Ok(result);
        }
    }
}