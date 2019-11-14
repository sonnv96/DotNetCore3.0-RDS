using System;
using System.Collections.Generic;
using System.Text;

namespace RDS.Core.Entities.Users
{
    public class RolePermission
    {
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
