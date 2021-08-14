using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WildAPI.Models;
using WildAPI.Models.Dtos;

namespace API_CORE
{
    public class MyMapper : Profile
    {
        public MyMapper()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            //CreateMap<NationalPark, NationalParkDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name + src.Id));



        }
    }
}
