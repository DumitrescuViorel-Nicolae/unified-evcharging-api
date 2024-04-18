using AutoMapper;
using Domain.DTOs;
using Infrastructure.Data;

namespace Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDTO>();
        }
    }
}
