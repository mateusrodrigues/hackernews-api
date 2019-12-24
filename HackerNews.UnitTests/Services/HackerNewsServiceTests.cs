using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HackerNews.Domain;
using HackerNews.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace HackerNews.UnitTests.Services
{
    public class HackerNewsServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandler;
        private readonly Mock<ICacheService> _cache;
        private readonly IHackerNewsService _service;

        public HackerNewsServiceTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();
            _cache = new Mock<ICacheService>();

            var httpClient = new HttpClient(_httpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://example.com")
            };
            _service = new HackerNewsService(httpClient, _cache.Object);
        }
        
        [Fact]
        public void GetBestStoriesIds_CallCompletes_ReturnsAListOfTheBestStories()
        {
            var response = new int[]
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40
            };

            _httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                });

            var result = _service.GetBestStoriesIds().Result.ToList();

            Assert.IsType<List<int>>(result);
            Assert.Equal(response.Count(), result.Count());
        }

        [Fact]
        public void GetBestStoriesIds_NetworkError_ReturnsEmptyList()
        {
            _httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Throws<HttpRequestException>();

            var result = _service.GetBestStoriesIds().Result.ToList();
            
            Assert.IsType<List<int>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetStoryDetail_StoryIsNotInCache_SaveToCacheAndReturnStoryFromApi()
        {
            var response = "{\"by\": \"me\",\"descendants\": 1,\"id\": 1,\"kids\": [],\"score\": 1," +
                           "\"time\": 1570887781,\"title\": \"A uBlock...\",\"type\": \"story\"," +
                           "\"url\": \"http://example.com\"}";
            
            _cache.Setup(c => c.TryRead(1, out response))
                .Returns(false);

            _httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response)
                });
            
            var result = _service.GetStoryDetail(1).Result;
            var shouldBe = JsonConvert.DeserializeObject<Story>(response);

            _cache.Verify(c => c.Set(1, response, It.IsAny<int>()));
            Assert.IsType<Story>(result);
            Assert.Equal(shouldBe.Score, result.Score);
            Assert.Equal(shouldBe.Time, result.Time);
            Assert.Equal(shouldBe.Title, result.Title);
            Assert.Equal(shouldBe.Uri, result.Uri);
            Assert.Equal(shouldBe.CommentCount, result.CommentCount);
            Assert.Equal(shouldBe.PostedBy, result.PostedBy);
        }

        [Fact]
        public void GetStoryDetail_StoryIsInCache_ReadFromCacheAndReturnStory()
        {
            var cacheContent = "{\"by\": \"me\",\"descendants\": 1,\"id\": 1,\"kids\": [],\"score\": 1," +
                               "\"time\": 1570887781,\"title\": \"A uBlock...\",\"type\": \"story\"," +
                               "\"url\": \"http://example.com\"}";
            _cache.Setup(c => c.TryRead(1, out cacheContent))
                .Returns(true);

            var result = _service.GetStoryDetail(1).Result;
            var shouldBe = JsonConvert.DeserializeObject<Story>(cacheContent);

            Assert.IsType<Story>(result);
            Assert.Equal(shouldBe.Score, result.Score);
            Assert.Equal(shouldBe.Time, result.Time);
            Assert.Equal(shouldBe.Title, result.Title);
            Assert.Equal(shouldBe.Uri, result.Uri);
            Assert.Equal(shouldBe.CommentCount, result.CommentCount);
            Assert.Equal(shouldBe.PostedBy, result.PostedBy);
        }
    }
}