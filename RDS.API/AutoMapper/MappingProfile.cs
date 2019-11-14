using AutoMapper;
using RDS.API.Models.Users;
using RDS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.API.AutoMapper
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<User, Response>();
            CreateMap<Response, Response2>();
        }
    }
}
