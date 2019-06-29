namespace MyCorp.CityApi.Data.DTO.Response
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CityInformation
    {
        public Guid CityId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string CountryName { get; set; }
        public TouristRating TouristRating { get; set; }
        public DateTime? DateEstablished { get; set; }
        public long? EstimatedPopulation { get; set; }
        public string ShortCountryCode { get; set; }
        public string LongCountryCode { get; set; }
        public IList<string> CurrencyCodes { get; set; }
        public string WeatherDescription { get; set; }
    }
}