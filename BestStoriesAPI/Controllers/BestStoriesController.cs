using BestStoriesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;

namespace BestStoriesAPI.Controllers;

[ApiController]
[Route("/api")]
[AllowAnonymous]
[Produces("application/json")]
public class BestStoriesController : ControllerBase
{
	private readonly IBestStoriesApiService _bestStoriesService;
	private readonly ILogger<BestStoriesController> _logger;

    public BestStoriesController(ILogger<BestStoriesController> logger, IBestStoriesApiService service)
    {
        _logger = logger;
		_bestStoriesService = service;
    }

	[HttpGet("best-stories/{limit}", Name = "BestStories_Get")]
	[SwaggerResponse((int)HttpStatusCode.OK, "Best stories", typeof(List<DTOs.BestNewsStory>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "No stories found")]
	[SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid Stories limit")]
	[SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unable to fetch best stories")]
	public async Task<IActionResult> Get(int limit)
	{
        _logger.LogInformation("Fetching Best Stories ...");
		try
        {
			_logger.LogInformation("Getting {0} best stories", limit);
			var result = await _bestStoriesService.GetBestStories(limit);

            return result.Any() ? Ok(result) : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching best stories...", ex.Message);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

}
