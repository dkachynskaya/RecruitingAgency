using AutoMapper;
using BLL.DTOs;
using DAL.Identity;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class MapDTOModels: Profile
    {
        public MapDTOModels()
        {
            CreateMap<Ad, AdDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>();

            CreateMap<UserDTO, ApplicationUser>().ForMember(x => x.UserName, opt => opt.MapFrom(y => y.Login))
                .ForMember(x => x.Roles, opt => opt.Ignore());
        }
    }
}
