using RDS.Core.Entities.Users;
using RDS.Framework.Services.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.Framework.Services.Users
{
    public interface IUserService : IBaseService<User>
    {
        IQueryable<User> FilterUserListByOptions();
        Task<bool> CheckExistAsync(Func<User, bool> fieldCheck, int? entityId = null);
    }
}
