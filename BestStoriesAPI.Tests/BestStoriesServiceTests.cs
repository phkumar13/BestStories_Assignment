using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BestStoriesAPI.Data;
using BestStoriesAPI.Data.Models;
using BestStoriesAPI.Services;
using Moq;
using Xunit;

namespace BestStoriesAPI.Tests;

public class BestStoriesServiceTests
{
    [Fact]
    public async Task GetBestStories_ShouldReturnBestStoriesOrderedAndTakeLimitIntoAccount()
    {
        // Arrange
        var bestStoriesApiMock = new Mock<IBestStoriesApi>();
        bestStoriesApiMock.Setup(api => api.GetBestStoryIdsAsync())
            .ReturnsAsync(new List<int> { 1, 2, 3, 4, 5 });

        bestStoriesApiMock.Setup(api => api.GetStoryAsync(It.IsAny<int>()))
            .ReturnsAsync((int storyId) => new BestNewsStory
            {
                By = "test",
                Descendants = 10,
                Id = storyId,
                Kids = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                Score = storyId * 100,
                Time = 1234567890 + storyId,
                Title = $"Test Story {storyId}",
                Type = "story",
                Url = $"https://testendpoint.com/{storyId}"
            });

        var hackerNewsService = new BestStoriesApiService(bestStoriesApiMock.Object);

        // Act
        var result = await hackerNewsService.GetBestStories(3);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);

        Assert.Equal("test", result[0].PostedBy);
        Assert.Equal("test", result[1].PostedBy);
        Assert.Equal("test", result[2].PostedBy);

        Assert.Equal(10, result[0].CommentCount);
        Assert.Equal(10, result[1].CommentCount);
        Assert.Equal(10, result[2].CommentCount);

        Assert.Equal(300, result[0].Score);
        Assert.Equal(200, result[1].Score);
        Assert.Equal(100, result[2].Score);

        Assert.Equal("Test Story 3", result[0].Title);
        Assert.Equal("Test Story 2", result[1].Title);
        Assert.Equal("Test Story 1", result[2].Title);

        Assert.Equal(new DateTime(2009, 2, 13, 23, 31, 33), result[0].Time);
        Assert.Equal(new DateTime(2009, 2, 13, 23, 31, 32), result[1].Time);
        Assert.Equal(new DateTime(2009, 2, 13, 23, 31, 31), result[2].Time);

        Assert.Equal("https://testendpoint.com/3", result[0].Uri);
        Assert.Equal("https://testendpoint.com/2", result[1].Uri);
        Assert.Equal("https://testendpoint.com/1", result[2].Uri);
    }
}