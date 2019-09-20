using AutoMapper;
using DTOs;
using Entities;

namespace Helpers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}