
namespace WeatherAPI.Services
{
    public interface IWeatherApiService
    {
        Task<WeatherApiResponse> SearchByWeatherAsync(string title);
    }
}