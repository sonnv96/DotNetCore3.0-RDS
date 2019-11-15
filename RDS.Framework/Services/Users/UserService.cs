using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RDS.Core;
using RDS.Core.Entities.Users;
using RDS.Framework.Helpers;
using RDS.Framework.Repositories;
using RDS.Framework.Services.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.Framework.Services.Users
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;

        public UserService(
           IConfiguration config,
           IHostEnvironment env,
           IRepository<User> tRespository) : base(tRespository)
        {
            _config = config;
            _env = env;
        }


        public IPagedList<User> Search(int pageIndex = 0, int pageSize = int.MaxValue, string textSearch = null, string propertySorting = null, bool orderDescending = false, bool includeCurrentUser = true)
        {
            var query = _tRepository.Table;
            var a = query.ToList();
            query = query.Where(x => !x.Username.ToLower().Equals("admin"));
            if (!string.IsNullOrEmpty(textSearch))
                query = query.Where(x => x.Username.Contains(textSearch) || x.FirstName.Contains(textSearch) || x.Email.Contains(textSearch));
            return new PagedList<User>(query.OrderByDynamic(propertySorting, "Username", orderDescending), pageIndex, pageSize) as IPagedList<User>;
        }


    }


}


