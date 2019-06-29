namespace MyCorp.CityApi.Models.Services.Weather
{
    using Newtonsoft.Json;

    public partial class Wind
    {
        [JsonProperty("speed", NullValueHandling = NullValueHandling.Ignore)]
        public double? Speed { get; set; }

        [JsonProperty("deg", NullValueHandling = NullValueHandling.Ignore)]
        public long? Deg { get; set; }
    }
}