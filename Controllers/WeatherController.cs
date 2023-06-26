using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorProject.Services;

namespace SensorProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private CancellationTokenSource _cancellationTokenSource;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [HttpGet("{city}")]
        public IActionResult GetWeather(string city)
        {
            var weatherData = _weatherService.GetWeatherData(city);

            if (weatherData == null)
            {
                return NotFound();
            }

            return Ok(weatherData);
        }

        [HttpPost("{city}/start-automatic-updates")]
        public IActionResult StartAutomaticUpdates(string city)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }

            TimeSpan interval = TimeSpan.FromMinutes(5);
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            Task.Run(() => _weatherService.StartAutomaticUpdates(city, interval, cancellationToken));

            return Ok("Automatic updates started.");
        }

        [HttpPost("stop-automatic-updates")]
        public IActionResult StopAutomaticUpdates()
        {
            _cancellationTokenSource.Cancel();
            return Ok("Automatic updates stopped.");
        }
    }
}
