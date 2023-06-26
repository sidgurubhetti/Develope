using SensorProject.Entities;
using SensorProject.IRepository;

namespace SensorProject.Services
{
    public class WeatherService
    {
        private readonly IWeatherRepository _weatherRepository;

        public WeatherService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        public WeatherData GetWeatherData(string city)
        {
            return _weatherRepository.GetWeatherData(city);
        }

        public async Task StartAutomaticUpdates(string city, TimeSpan interval, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var updatedData = FetchUpdatedWeatherData(city);
                _weatherRepository.UpdateWeatherData(updatedData);

                await Task.Delay(interval, cancellationToken);
            }
        }

        private WeatherData FetchUpdatedWeatherData(string city)
        {
            // TODO: Implement logic to fetch updated weather data from the database or external source
            // For now, return a dummy WeatherData object

            return new WeatherData
            {
                City = city,
                Temperature = 28.0m,
                Precipitation = 5.0m,
                Humidity = 80.0m
            };
        }
    }
}
