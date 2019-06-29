namespace MyCorp.CityApi.Models.Services.Weather
{
    using Newtonsoft.Json;

    public partial class Coord
    {
        [JsonProperty("lon", NullValueHandling = NullValueHandling.Ignore)]
        public double? Lon { get; set; }

        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public double? Lat { get; set; }
    }
}