using BestStoriesAPI.Data;
using BestStoriesAPI.DTOs;

namespace BestStoriesAPI.Services;

public class BestStoriesApiService : IBestStoriesApiService
{
    private readonly IBestStoriesApi _bestStoriesApi;

    public BestStoriesApiService(IBestStoriesApi bestStoriesApi)
    {
        _bestStoriesApi = bestStoriesApi;
    }

    public async Task<List<BestNewsStory>> GetBestStories(int limit)
    {
        var result = new List<Data.Models.BestNewsStory?>();
        var bestStoriesIds = (await _bestStoriesApi.GetBestStoryIdsAsync() ?? throw new InvalidOperationException()).Take(limit);

        foreach (var id in bestStoriesIds)
        {
            var story = await _bestStoriesApi.GetStoryAsync(id);
            result.Add(story);
        }
        return BuildDTO(result);
    }

    private List<BestNewsStory> BuildDTO(List<Data.Models.BestNewsStory?> stories)
    {
        return stories.Select(story => new BestNewsStory
        {
            Title = story?.Title,
            Uri = story?.Url,
            PostedBy = story?.By,
            Time = story?.Time != null ? DateTimeOffset.FromUnixTimeSeconds(story.Time).DateTime : null,
            Score = story?.Score,
            CommentCount = story?.Kids?.Length ?? 0
        }).OrderByDescending(story => story.Score).ToList();
    }
}