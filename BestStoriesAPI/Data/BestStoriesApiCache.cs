using BestStoriesAPI.Data.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BestStoriesAPI.Data;

public class BestStoriesApiCache : IBestStoriesApiCache
{
	private readonly IBestStoriesApi _apiClient;
	private readonly IMemoryCache _cache;
	private readonly CacheOptions _cacheOptions;
	private readonly ILogger<BestStoriesApiCache> _logger;

	public BestStoriesApiCache(IBestStoriesApi apiClient, IMemoryCache cache, IOptions<CacheOptions> cacheOptions,
		ILogger<BestStoriesApiCache> logger)
	{
		_apiClient = apiClient;
		_cache = cache;
		_logger = logger;
		_cacheOptions = cacheOptions.Value;
	}

	public async Task<List<int>?> GetBestStoryIdsAsync()
	{
		const string cacheKey = "BestStoryIds";
		if (_cache.TryGetValue(cacheKey, out List<int>? storyIds))
		{
			_logger.LogInformation("Fetching best story ids from cache.");
			return storyIds;
		}

		try
		{
			_logger.LogInformation("Fetching best story ids from API.");
			storyIds = await _apiClient.GetBestStoryIdsAsync();
			var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_cacheOptions.StoryIdsExpirationTimeMinutes));
			_cache.Set(cacheKey, storyIds, cacheOptions);
			return storyIds;
		}
		catch (Exception ex)
		{
			_logger.LogError("Error fetching story ids from API...", ex.Message);
			throw;
		}
	}

	public async Task<BestNewsStory?> GetStoryAsync(int storyId)
	{
		var cacheKey = $"Story_{storyId}";
		if (_cache.TryGetValue(cacheKey, out BestNewsStory? story))
		{
			_logger.LogInformation("Fetching story {0} from cache.", storyId);
			return story;
		}

		try
		{
			_logger.LogInformation("Fetching story {0} from API.", storyId);
			story = await _apiClient.GetStoryAsync(storyId);
			var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_cacheOptions.StoryExpirationTimeMinutes));
			_cache.Set(cacheKey, story, cacheOptions);
			return story;
		}
		catch (Exception ex)
		{
			_logger.LogError(string.Format("Error fetching story {0}...", storyId), ex.Message);
			throw;
		}
	}
}