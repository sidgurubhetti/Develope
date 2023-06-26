using SensorProject.Entities;

namespace SensorProject.IRepository
{
    public interface IWeatherRepository //: IRepository<WeatherData>
    {
        WeatherData GetWeatherData(string city);

        void UpdateWeatherData(WeatherData weatherData);
    }
}
