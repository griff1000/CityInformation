
namespace MyCorp.CityApi.Models.Services.Countries
{
    using System;
    using Newtonsoft.Json;
    using Utilities;

    public partial class Country
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("topLevelDomain", NullValueHandling = NullValueHandling.Ignore)]
        public string[] TopLevelDomain { get; set; }

        [JsonProperty("alpha2Code", NullValueHandling = NullValueHandling.Ignore)]
        public string Alpha2Code { get; set; }

        [JsonProperty("alpha3Code", NullValueHandling = NullValueHandling.Ignore)]
        public string Alpha3Code { get; set; }

        [JsonProperty("callingCodes", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(DecodeArrayConverter))]
        public long[] CallingCodes { get; set; }

        [JsonProperty("capital", NullValueHandling = NullValueHandling.Ignore)]
        public string Capital { get; set; }

        [JsonProperty("altSpellings", NullValueHandling = NullValueHandling.Ignore)]
        public string[] AltSpellings { get; set; }

        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        public string Region { get; set; }

        [JsonProperty("subregion", NullValueHandling = NullValueHandling.Ignore)]
        public string Subregion { get; set; }

        [JsonProperty("population", NullValueHandling = NullValueHandling.Ignore)]
        public long? Population { get; set; }

        [JsonProperty("latlng", NullValueHandling = NullValueHandling.Ignore)]
        public long[] Latlng { get; set; }

        [JsonProperty("demonym", NullValueHandling = NullValueHandling.Ignore)]
        public string Demonym { get; set; }

        [JsonProperty("area", NullValueHandling = NullValueHandling.Ignore)]
        public long? Area { get; set; }

        [JsonProperty("gini", NullValueHandling = NullValueHandling.Ignore)]
        public double? Gini { get; set; }

        [JsonProperty("timezones", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Timezones { get; set; }

        [JsonProperty("borders", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Borders { get; set; }

        [JsonProperty("nativeName", NullValueHandling = NullValueHandling.Ignore)]
        public string NativeName { get; set; }

        [JsonProperty("numericCode", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? NumericCode { get; set; }

        [JsonProperty("currencies", NullValueHandling = NullValueHandling.Ignore)]
        public Currency[] Currencies { get; set; }

        [JsonProperty("languages", NullValueHandling = NullValueHandling.Ignore)]
        public Language[] Languages { get; set; }

        [JsonProperty("translations", NullValueHandling = NullValueHandling.Ignore)]
        public Translations Translations { get; set; }

        [JsonProperty("flag", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Flag { get; set; }

        [JsonProperty("regionalBlocs", NullValueHandling = NullValueHandling.Ignore)]
        public RegionalBloc[] RegionalBlocs { get; set; }

        [JsonProperty("cioc", NullValueHandling = NullValueHandling.Ignore)]
        public string Cioc { get; set; }
    }
}
