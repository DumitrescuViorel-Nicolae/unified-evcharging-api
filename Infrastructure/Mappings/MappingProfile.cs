using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<EVStationDTO, EVStation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.TotalNumberOfConnectors, opt => opt.MapFrom(src => src.TotalNumberOfConnectors))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Contacts.Phone))
            .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Contacts.Website))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Position.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Position.Longitude))
            .ForMember(dest => dest.StripeAccountID, opt => opt.MapFrom(src => src.StripeAccountID));
        }
    }
}
