using BestStoriesAPI.DTOs;

namespace BestStoriesAPI.Services;

public interface IBestStoriesApiService
{
    Task<List<BestNewsStory>> GetBestStories(int limit);
}