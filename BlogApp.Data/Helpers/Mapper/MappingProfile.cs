using AutoMapper;
using BlogApp.Data.Dto.User;
using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data.Helpers.Mapper
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Mapper
        /// </summary>
        public MappingProfile()
        {
            //User
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<RegisterDto, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
