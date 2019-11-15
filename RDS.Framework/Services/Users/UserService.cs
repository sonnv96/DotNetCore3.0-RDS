using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RDS.Core;
using RDS.Core.Entities.Users;
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
        private readonly IRepository<User> _userRespository;

        public UserService(
           RDSContext context,
           IConfiguration config,
           IHostEnvironment env,
           IRepository<User> userRespository) :base(context)
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

        public virtual async Task<bool> CheckExistAsync(Func<User, bool> fieldCheck, int? entityId = null)
        {
            var query = _userRespository.Query();
            if (entityId.HasValue)
            {
                query = query.Where(x => x.Id != entityId);
            }
            return await Task.FromResult(query.Any(fieldCheck));
        }




    }


}


