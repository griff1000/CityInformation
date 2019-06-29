namespace MyCorp.CityApi.Models.Services.Countries
{
    using Newtonsoft.Json;

    public partial class Language
    {
        [JsonProperty("iso639_1", NullValueHandling = NullValueHandling.Ignore)]
        public string Iso6391 { get; set; }

        [JsonProperty("iso639_2", NullValueHandling = NullValueHandling.Ignore)]
        public string Iso6392 { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("nativeName", NullValueHandling = NullValueHandling.Ignore)]
        public string NativeName { get; set; }
    }
}