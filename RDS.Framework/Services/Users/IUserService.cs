using RDS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDS.Framework.Services.Users
{
    public interface IUserService
    {
        IQueryable<User> GetUserListByOptions();
    }
}
