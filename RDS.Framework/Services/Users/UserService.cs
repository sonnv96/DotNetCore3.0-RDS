using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RDS.Core.Entities.Users;
using RDS.Framework.Repositories;
using System.Linq;

namespace RDS.Framework.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;
        private readonly IRepository<User> _userRespository;

        public UserService(
           IConfiguration config,
           IHostEnvironment env,
           IRepository<User> userRespository)
        {
            _config = config;
            _env = env;
            _userRespository = userRespository;
        }


        public IQueryable<User> FilterUserListByOptions()
        {
            // init query
            var query = _userRespository.Query().AsNoTracking();
            return query;
        }


    }


}


