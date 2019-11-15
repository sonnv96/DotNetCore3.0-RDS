using RDS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDS.Core.Entities.Users
{
    public class Role : IBaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// System name
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Is Active
        /// </summary>
        public bool IsActive { get; set; }
        private ICollection<RolePermission> _rolePermission;

        /// <summary>
        /// roles permission
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions
        {
            get { return _rolePermission ?? (_rolePermission = new List<RolePermission>()); }
            set { _rolePermission = value; }
        }
        private ICollection<UserRole> _userRole;

        /// <summary>
        /// User roles
        /// </summary>
        public virtual ICollection<UserRole> UserRoles
        {
            get { return _userRole ?? (_userRole = new List<UserRole>()); }
            set { _userRole = value; }
        }

        public bool Deleted { get; set; }
    }
}
