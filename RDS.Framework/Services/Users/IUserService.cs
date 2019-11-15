using RDS.Core.Entities.Users;
using RDS.Framework.Helpers;
using RDS.Framework.Services.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.Framework.Services.Users
{
    public interface IUserService : IBaseService<User>
    {
        IPagedList<User> Search(int pageIndex = 0, int pageSize = int.MaxValue, string textSearch = null, string propertySorting = null, bool orderDescending = false, bool includeCurrentUser = true);
    }
}
