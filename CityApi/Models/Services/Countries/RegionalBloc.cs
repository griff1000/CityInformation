namespace MyCorp.CityApi.Models.Services.Countries
{
    using Newtonsoft.Json;

    public partial class RegionalBloc
    {
        [JsonProperty("acronym", NullValueHandling = NullValueHandling.Ignore)]
        public string Acronym { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("otherAcronyms", NullValueHandling = NullValueHandling.Ignore)]
        public string[] OtherAcronyms { get; set; }

        [JsonProperty("otherNames", NullValueHandling = NullValueHandling.Ignore)]
        public string[] OtherNames { get; set; }
    }
}