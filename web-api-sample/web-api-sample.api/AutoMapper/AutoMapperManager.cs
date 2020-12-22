using AutoMapper;
using web_api_sample.api.Models.Dtos;
using web_api_sample.api.Models.Entities;

namespace web_api_sample.api.AutoMapper
{
    public class AutoMapperManager : Profile
    {
        public AutoMapperManager()
        {
            //Role
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            //User
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}