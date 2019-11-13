using AutoMapper;
using RDS.Core.Entities;
using RDS.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.API.Models.Users
{
    public class Response
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
    }

    public class Response2
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, Response>(); // means you want to map from User to UserDTO
            CreateMap<Response, User>(); // means you want to map from User to UserDTO
            CreateMap<Response, Response2>(); // means you want to map from User to UserDTO
        }
    }
}
