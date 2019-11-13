using RDS.Core.Entities;
using RDS.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.Framework.Services.Users
{
    public interface IUserService
    {
        IQueryable<User> FilterUserListByOptions();
    }
}
