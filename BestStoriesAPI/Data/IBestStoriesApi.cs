using BestStoriesAPI.Data.Models;

namespace BestStoriesAPI.Data;

public interface IBestStoriesApi
{
    Task<List<int>?> GetBestStoryIdsAsync();
    Task<BestNewsStory?> GetStoryAsync(int storyId);
}