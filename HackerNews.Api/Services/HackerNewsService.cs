using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HackerNews.Api.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HackerNews.Api.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private const int SlidingExpiration = 6;
        
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cache;

        public HackerNewsService(HttpClient httpClient,
            ICacheService cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }
        
        public async Task<IEnumerable<int>> GetBestStoriesIds()
        {
            var response = "";
            try
            {
                response = await _httpClient.GetStringAsync("beststories.json");
            }
            catch (HttpRequestException)
            {
                return new int[] { };
            }

            return JsonConvert.DeserializeObject<IEnumerable<int>>(response);
        }

        public async Task<Story> GetStoryDetail(int id)
        {
            // Key in cache. Return saved data.
            if (_cache.TryRead(id, out var result)) 
                return JsonConvert.DeserializeObject<Story>(result);
            
            // Key is not in cache. Fetch data from API.
            result = await _httpClient.GetStringAsync($"item/{id}.json");

            // Save data to cache
            _cache.Set(id, result, SlidingExpiration);
            
            return JsonConvert.DeserializeObject<Story>(result);
        }
    }
}