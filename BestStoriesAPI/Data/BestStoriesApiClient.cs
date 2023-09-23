#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using BestStoriesAPI.Controllers;
using BestStoriesAPI.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BestStoriesAPI.Data;

public class BestStoriesApiClient : IBestStoriesApiClient
{
	private readonly string BaseUrl;
	private readonly HttpClient _httpClient;

	public BestStoriesApiClient(HttpClient httpClient, IConfiguration configuration)
	{
		_httpClient = httpClient;
		BaseUrl = configuration.GetSection("MySettings").GetSection("BaseURL").Value;
	}

	public async Task<List<int>> GetBestStoryIdsAsync()
	{
		return JsonSerializer.Deserialize<List<int>>(await GetStoriesAsync($"{BaseUrl}/beststories.json"));
	}

	public async Task<BestNewsStory> GetStoryAsync(int storyId)
	{
		return JsonSerializer.Deserialize<BestNewsStory>(await GetStoriesAsync($"{BaseUrl}/item/{storyId}.json"));
	}

	private async Task<string> GetStoriesAsync(string url)
	{
		var response = await _httpClient.GetAsync(url);
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadAsStringAsync();
	}
}