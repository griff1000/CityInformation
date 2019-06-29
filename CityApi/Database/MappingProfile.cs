namespace MyCorp.CityApi.Database
{
    using AutoMapper;
    using Data.DTO.Request;
    using Data.DTO.Response;
    using Models.Database;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<City, CityInformation>();
            CreateMap<CityToAdd, City>();
        }
    }
}