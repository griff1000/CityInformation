namespace MyCorp.CityApi.Services.Weather
{
    using System.Threading.Tasks;
    using Models.Services.Weather;

    public interface IWeatherApiClient
    {
        Task<Envelope> GetWeatherForCityAsync(string city, string appId);
    }
}