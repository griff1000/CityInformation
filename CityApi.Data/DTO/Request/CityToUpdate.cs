namespace MyCorp.CityApi.Data.DTO.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CityToUpdate
    {
        [EnumDataType(typeof(TouristRating))]
        [Range(1, 5)]
        public int TouristRating { get; set; }

        public DateTime DateEstablished { get; set; }
    }
}