global using Microsoft.EntityFrameworkCore;
using BestStoriesAPI.Data;
using BestStoriesAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IBestStoriesApiClient, BestStoriesApiClient>();
builder.Services.AddScoped<IBestStoriesApi, BestStoriesApiClient>();
builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection("CacheOptions"));
builder.Services.AddScoped<IBestStoriesApiCache, BestStoriesApiCache>();
builder.Services.AddScoped<IBestStoriesApiService, BestStoriesApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
