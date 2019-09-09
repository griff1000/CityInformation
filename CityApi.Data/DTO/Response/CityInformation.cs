namespace MyCorp.CityApi.Data.DTO.Response
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract(Name="CityInformation", Namespace = "http://www.griffico.com")]
    public class CityInformation
    {
        [DataMember]
        public Guid CityId { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public TouristRating TouristRating { get; set; }
        [DataMember]
        public DateTime? DateEstablished { get; set; }
        [DataMember]
        public long? EstimatedPopulation { get; set; }
        [DataMember]
        public string ShortCountryCode { get; set; }
        [DataMember]
        public string LongCountryCode { get; set; }
        [DataMember]
        public IList<string> CurrencyCodes { get; set; }
        [DataMember]
        public string WeatherDescription { get; set; }
    }
}