using RDS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDS.Core.Entities.Users
{
    public class ActionName : IBaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// controller name
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// action name
        /// </summary>
        public string Name { get; set; }


        private ICollection<ActionNamePermission> _actionNamePermission;
        /// <summary>
        /// Action Name Permission
        /// </summary>
        public virtual ICollection<ActionNamePermission> ActionNamePermissions
        {
            get { return _actionNamePermission ?? (_actionNamePermission = new List<ActionNamePermission>()); }
            protected set { _actionNamePermission = value; }
        }
    }
}
