using AronWebAPI.DTOs;
using AronWebAPI.Entites;
using AutoMapper;

namespace AronWebAPI.Hellpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegistrationDTO, User>()
                .ForMember(dst => dst.CreatedOn, opt => opt.MapFrom(
                    src => DateTime.UtcNow))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(
                    src => "fsfdf"));


            CreateMap<User, UserResponseDTO>()
               .ForMember(dst => dst.Active, opt => opt.MapFrom(
                   src => src.RevokedOn == null));
               
        }
    }
}
