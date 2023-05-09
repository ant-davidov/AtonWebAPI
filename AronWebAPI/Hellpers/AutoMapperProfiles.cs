using AronWebAPI.DTOs;
using AronWebAPI.Entites;
using AtonWebAPI.DTOs;
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
                    src => "fsfdf"))
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(
                    src => src.Login));

            CreateMap<User, UserResponseForAdmin>()
                  .ForMember(dst => dst.IsActive, opt => opt.MapFrom(
                    src => src.RevokedBy == null));

            CreateMap<User, UserResponseForUser>()
                 .ForMember(dst => dst.Token, opt => opt.MapFrom(
                    src => String.Empty));


        }
    }
}
