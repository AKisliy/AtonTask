using AutoMapper;
using UserService.Core.Models;
using UserService.DataAccess;
using UserService.WebApi.Dto.Request;

namespace UserService.WebApi.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<UserRegisterRequest, User>();
        }
    }
}