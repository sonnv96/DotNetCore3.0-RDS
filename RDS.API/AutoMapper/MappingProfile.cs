using AutoMapper;
using RDS.API.Models.Users;
using RDS.Core.Entities.Users;

namespace RDS.API.AutoMapper
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<User, UserModel>();
        }
    }
}
