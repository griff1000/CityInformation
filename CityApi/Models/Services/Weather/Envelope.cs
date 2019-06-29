namespace MyCorp.CityApi.Models.Services.Weather
{
    using Newtonsoft.Json;

    public class Envelope
    {
        [JsonProperty("coord", NullValueHandling = NullValueHandling.Ignore)]
        public Coord Coord { get; set; }

        [JsonProperty("weather", NullValueHandling = NullValueHandling.Ignore)]
        public Weather[] Weather { get; set; }

        [JsonProperty("base", NullValueHandling = NullValueHandling.Ignore)]
        public string Base { get; set; }

        [JsonProperty("main", NullValueHandling = NullValueHandling.Ignore)]
        public Main Main { get; set; }

        [JsonProperty("visibility", NullValueHandling = NullValueHandling.Ignore)]
        public long? Visibility { get; set; }

        [JsonProperty("wind", NullValueHandling = NullValueHandling.Ignore)]
        public Wind Wind { get; set; }

        [JsonProperty("clouds", NullValueHandling = NullValueHandling.Ignore)]
        public Clouds Clouds { get; set; }

        [JsonProperty("dt", NullValueHandling = NullValueHandling.Ignore)]
        public long? Dt { get; set; }

        [JsonProperty("sys", NullValueHandling = NullValueHandling.Ignore)]
        public Sys Sys { get; set; }

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public long? Timezone { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("cod", NullValueHandling = NullValueHandling.Ignore)]
        public long? Cod { get; set; }
    }
}