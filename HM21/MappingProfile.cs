using AutoMapper;
using HM21.Entity.Model;
using HM21.Entity.ModelDTO;
using HM21.Entity.ModelDTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HM21
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateFoodDto, Food>();
            CreateMap<UpdateFoodDTO, Food>().ReverseMap();
            CreateMap<UserRegistration, User>();
        }
    }
}
