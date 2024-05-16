using AutoMapper;
using UserService.Core.Models;
using UserService.DataAccess;
using UserService.WebApi.Dto.Request;
using UserService.WebApi.Dto.Response;

namespace UserService.WebApi.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<UserRegisterRequest, User>();
            CreateMap<User, UserResponse>();
            CreateMap<User, UserByLoginResponse>()
                .ForMember(u => u.IsActive, opt => opt.MapFrom(u => u.RevokedOn == null));
        }
    }
}