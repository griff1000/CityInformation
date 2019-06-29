namespace MyCorp.CityApi.Models.Database
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.DTO;

    [Serializable]
    public class City
    {
        [Key] public Guid CityId { get; set; }

        [Required] public string Name { get; set; }

        public string State { get; set; }
        public TouristRating TouristRating { get; set; }
        public DateTime DateEstablished { get; set; }

        [Required] public string CountryName { get; set; }
    }
}