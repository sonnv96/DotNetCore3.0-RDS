using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RDS.Core.Entities;
using RDS.Framework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.Framework.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;
        private readonly IRepository<User> _userRespository;

        public UserService(
           IConfiguration config,
           IHostingEnvironment env,
           IRepository<User> userRespository)
        {
            _config = config;
            _env = env;
            _userRespository = userRespository;
        }


        public IQueryable<User> GetUserListByOptions()
        {

            return  _userRespository.Query().AsNoTracking();

        }

    }
}
