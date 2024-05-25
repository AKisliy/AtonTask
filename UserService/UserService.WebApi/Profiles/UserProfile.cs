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
            CreateMap<User, UserEntity>()
                .ForMember(u => u.BirthDate, o => o.MapFrom(src => src.BirthDate.HasValue
                                              ? DateTime.SpecifyKind(src.BirthDate.Value, DateTimeKind.Utc)
                                              : (DateTime?)null))
                .ForMember(u => u.ModifiedOn, o => o.MapFrom(src => src.ModifiedOn.HasValue
                                              ? DateTime.SpecifyKind(src.ModifiedOn.Value, DateTimeKind.Utc)
                                              : (DateTime?)null))
                .ForMember(u => u.RevokedOn, o => o.MapFrom(src => src.RevokedOn.HasValue
                                              ? DateTime.SpecifyKind(src.RevokedOn.Value, DateTimeKind.Utc)
                                              : (DateTime?)null))
                .ForMember(u => u.CreatedOn, o => o.MapFrom(src => DateTime.SpecifyKind(src.CreatedOn, DateTimeKind.Utc)));
            CreateMap<UserEntity, User>();
            CreateMap<UserRegisterRequest, User>();
            CreateMap<User, UserResponse>()
                .ForMember(u => u.BirthDate, o => o.MapFrom(src => src.BirthDate.HasValue 
                                              ? DateOnly.FromDateTime(src.BirthDate.Value)
                                              : (DateOnly?)null));
            CreateMap<User, UserByLoginResponse>()
                .ForMember(u => u.IsActive, opt => opt.MapFrom(u => u.RevokedOn == null))
                .ForMember(u => u.BirthDate, o => o.MapFrom(src => src.BirthDate.HasValue 
                                              ? DateOnly.FromDateTime(src.BirthDate.Value)
                                              : (DateOnly?)null));
        }
    }
}