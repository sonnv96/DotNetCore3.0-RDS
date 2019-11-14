using System;
using System.Collections.Generic;
using System.Text;

namespace RDS.Core.Entities.Users
{
    public class ActionNamePermission
    {
        public int ActionNameId { get; set; }
        public virtual ActionName ActionName { get; set; }

        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
