using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WildAPI.Models;
using WildAPI.Models.Dtos;

namespace API_CORE
{
    public class AutoMapperConfig
    {
        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                //way one 
                cfg.CreateMap<NationalPark, NationalParkDto>();
                //way two
                cfg.AddProfile<AuthorMappingProfile>();
            }
           );

            return config;
        }
    }
    //way two 
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
