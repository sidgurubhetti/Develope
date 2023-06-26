using SensorProject.Entities;
using SensorProject.IRepository;

namespace SensorProject.Repository
{
    public class WeatherRepository : IWeatherRepository
    {
        private WeatherData _weatherData;

        public WeatherRepository()
        {
            // Initialize the weather data or retrieve it from the database
            // For simplicity, let's assume it's initially null
            _weatherData = null;
        }

        public WeatherData GetWeatherData(string city)
        {
            // Check if weather data is null or needs to be updated
            if (_weatherData == null)
            {
                // Fetch and update weather data from the database or external source
                _weatherData = FetchWeatherData(city);
            }

            return _weatherData;
        }

        public void UpdateWeatherData(WeatherData weatherData)
        {
            _weatherData = weatherData;
        }

        private WeatherData FetchWeatherData(string city)
        {
            // TODO: Implement logic to fetch weather data from the database or external source
            // For now, return a dummy WeatherData object

            return new WeatherData
            {
                City = city,
                Temperature = 25.5m,
                Precipitation = 10.2m,
                Humidity = 75.0m
            };
        }
    }
}
