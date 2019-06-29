namespace MyCorp.CityApi.Services.Weather
{
    using System.Threading.Tasks;
    using Models.Services.Weather;

    /// <summary>
    /// Handles interaction with the OpenWeather API
    /// </summary>
    public interface IWeatherApiClient
    {
        Task<Envelope> GetWeatherForCityAsync(string city, string appId);
    }
}