namespace MyCorp.CityApi.Models.Services.Countries
{
    using Newtonsoft.Json;

    public partial class Translations
    {
        [JsonProperty("de", NullValueHandling = NullValueHandling.Ignore)]
        public string De { get; set; }

        [JsonProperty("es", NullValueHandling = NullValueHandling.Ignore)]
        public string Es { get; set; }

        [JsonProperty("fr", NullValueHandling = NullValueHandling.Ignore)]
        public string Fr { get; set; }

        [JsonProperty("ja", NullValueHandling = NullValueHandling.Ignore)]
        public string Ja { get; set; }

        [JsonProperty("it", NullValueHandling = NullValueHandling.Ignore)]
        public string It { get; set; }

        [JsonProperty("br", NullValueHandling = NullValueHandling.Ignore)]
        public string Br { get; set; }

        [JsonProperty("pt", NullValueHandling = NullValueHandling.Ignore)]
        public string Pt { get; set; }
    }
}