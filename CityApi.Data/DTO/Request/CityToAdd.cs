namespace MyCorp.CityApi.Data.DTO.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CityToAdd
    {
        [Required] public string Name { get; set; }

        public string State { get; set; }
        public TouristRating TouristRating { get; set; }
        public DateTime DateEstablished { get; set; }

        [Required] public string CountryName { get; set; }
    }
}