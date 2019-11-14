using RDS.Core.Entities.Users;
using System.Linq;

namespace RDS.Framework.Services.Users
{
    public interface IUserService
    {
        IQueryable<User> FilterUserListByOptions();
    }
}
