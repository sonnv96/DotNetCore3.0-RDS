using RDS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDS.Core.Entities.Users
{
    public class Permission : BaseEntity
    {
      
        /// <summary>
        /// Permission name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Permission system name
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Permission category
        /// </summary>
        public string Category { get; set; }

        private ICollection<ActionNamePermission> _actionNamePermission;
        /// <summary>
        /// Action Name Permission
        /// </summary>
        public virtual ICollection<ActionNamePermission> ActionNamePermissions
        {
            get { return _actionNamePermission ?? (_actionNamePermission = new List<ActionNamePermission>()); }
            protected set { _actionNamePermission = value; }
        }

        private ICollection<RolePermission> _rolePermission;

        /// <summary>
        /// roles permission
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions
        {
            get { return _rolePermission ?? (_rolePermission = new List<RolePermission>()); }
            set { _rolePermission = value; }
        }

        /// <summary>
        /// Permission order
        /// </summary>
        /// 
        public int Order { get; set; }
    }
}
