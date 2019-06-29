namespace MyCorp.CityApi.Models.Services.Weather
{
    using Newtonsoft.Json;

    public partial class Main
    {
        [JsonProperty("temp", NullValueHandling = NullValueHandling.Ignore)]
        public double? Temp { get; set; }

        [JsonProperty("pressure", NullValueHandling = NullValueHandling.Ignore)]
        public long? Pressure { get; set; }

        [JsonProperty("humidity", NullValueHandling = NullValueHandling.Ignore)]
        public long? Humidity { get; set; }

        [JsonProperty("temp_min", NullValueHandling = NullValueHandling.Ignore)]
        public double? TempMin { get; set; }

        [JsonProperty("temp_max", NullValueHandling = NullValueHandling.Ignore)]
        public double? TempMax { get; set; }
    }
}