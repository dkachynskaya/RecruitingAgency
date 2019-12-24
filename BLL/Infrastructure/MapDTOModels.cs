using AutoMapper;
using BLL.DTOs;
using DAL.Identity;
using DAL.Models;

namespace BLL.Infrastructure
{
    public class MapDTOModels: Profile
    {
        public MapDTOModels()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Ad, AdDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>();

            CreateMap<UserDTO, ApplicationUser>().ForMember(x => x.UserName, opt => opt.MapFrom(y => y.Login))
                .ForMember(x => x.Roles, opt => opt.Ignore());
        }
    }
}
