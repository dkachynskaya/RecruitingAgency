using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Account;
using WebApi.Models.Category;
using WebApi.Models.JobOffer;
using WebApi.Models.User;

namespace WebApi.Mapper
{
    public class MapViewModels: AutoMapper.Profile
    {
        public MapViewModels()
        {
            CreateMap<RegisterViewModel, UserDTO>();
            CreateMap<UserDTO, ProfileViewModel>();
            CreateMap<EditProfileViewModel, UserDTO>();

            CreateMap<AdDTO, AdViewModel>();
            CreateMap<AdEditViewModel, AdDTO>();

            CreateMap<CategoryDTO, CategoryViewModel>();
            CreateMap<CategoryEditViewModel, CategoryDTO>();
        }
    }
}