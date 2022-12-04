
using AutoMapper;
using GainesOpusInstitute.DataEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GainesOpusInstitute.Web.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<LoginViewModel, User>().ReverseMap();
            CreateMap<RoleModel, Role>().ReverseMap();
            CreateMap<UserViewModel, User>().ReverseMap();
            CreateMap<RegisterViewModel, User>().ReverseMap();          
            CreateMap<ProfileModel, User>().ReverseMap();
        }
    }
}
