namespace MyCorp.CityApi.Models.Services.Weather
{
    using Newtonsoft.Json;

    public partial class Clouds
    {
        [JsonProperty("all", NullValueHandling = NullValueHandling.Ignore)]
        public long? All { get; set; }
    }
}