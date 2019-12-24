using System.Collections.Generic;
using System.Linq;
using HackerNews.Api.Controllers;
using HackerNews.Domain;
using HackerNews.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HackerNews.UnitTests.Controllers
{
    public class StoriesControllerTests
    {
        private readonly Mock<IHackerNewsService> _service;
        private readonly StoriesController _controller;
        
        public StoriesControllerTests()
        {
            _service = new Mock<IHackerNewsService>();
            _controller = new StoriesController(_service.Object);
        }

        [Fact]
        public void GetAll_WhenCalled_ReturnsThe200OkResult()
        {
            var okResult = _controller.GetAll();
            
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        
        [Fact]
        public void GetAll_WhenCalled_ReturnsThe20BestStories()
        {
            _service.Setup(s => s.GetBestStoriesIds())
                .ReturnsAsync(new int[]
                {
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
                    21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40
                });
            _service.Setup(s => s.GetStoryDetail(It.IsAny<int>()))
                .ReturnsAsync(new Story());

            var okResult = _controller.GetAll().Result as OkObjectResult;

            var items = Assert.IsType<List<Story>>(okResult.Value);
            Assert.Equal(20, items.Count());
        }
    }
}