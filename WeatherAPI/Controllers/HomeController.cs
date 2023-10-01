using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WeatherAPI.Models;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherApiService weatherApiService;

        public HomeController(IWeatherApiService weatherApiService)
        {
            this.weatherApiService = weatherApiService;
        }

        public IActionResult Index()
        {
            var json = HttpContext.Session.Get("WeatherHistory"); //Получаем данные JSON из сессии
            if (json != null)
            {
                var jsonData = Encoding.UTF8.GetString(json); //Преобразуем массив байтов в JSON
                var weather = JsonConvert.DeserializeObject<WeatherApiResponse>(jsonData); //Десериализуем JSON в объект класса
                ViewBag.cache = true;
                return View(weather);
            }
            ViewBag.cache = false;
            return View();
        }

        public async Task<IActionResult> Search(string title)
        {
            WeatherApiResponse result = null;
            try
            {
                result = await weatherApiService.SearchByWeatherAsync(title);
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
            }
            ViewBag.searchWeather = title;
            var json = JsonConvert.SerializeObject(result); //Сериализуем объект в JSON
            var bytes = Encoding.UTF8.GetBytes(json); //Преобразуем JSON в массив байтов
            HttpContext.Session.Set("WeatherHistory", bytes); //Сохраняем массив байтов в сессии
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}