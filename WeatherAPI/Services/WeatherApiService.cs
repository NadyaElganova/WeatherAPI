using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using WeatherAPI.Options;

namespace WeatherAPI.Services
{
    public class WeatherApiService : IWeatherApiService
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        private HttpClient _httpClient { get; set; }
        public WeatherApiService(IHttpClientFactory httpClientFactory, IOptions<WeatherApiOptions> weatherApiOptions)
        {
            BaseUrl = weatherApiOptions.Value.BaseUrl;
            ApiKey = weatherApiOptions.Value.ApiKey;
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<WeatherApiResponse> SearchByWeatherAsync(string title)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}?q={title}&appid={ApiKey}&units=metric");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<WeatherApiResponse>(json);
            if(result.cod != 200)
            {
                throw new Exception(result.message);
            }
            return result;
        }
    }
}
