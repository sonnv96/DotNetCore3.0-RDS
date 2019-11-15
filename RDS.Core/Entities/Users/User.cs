using RDS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDS.Core.Entities.Users
{
    public class User : IBaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// User Guid
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string SaltPassword { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Get or set position
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Gender identity : 1-Male; 2-Female
        /// </summary>
        public int GenderId { get; set; }

        /// <summary>
        /// Get or set position
        /// </summary>
        public int? CurrentLanguageId { get; set; }

        /// <summary>
        /// Language
        /// </summary>
        //public virtual Language CurrentLanguage { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        /// 
        public Gender Gender
        {
            get { return (Gender)GenderId; }
            set { GenderId = (int)value; }
        }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User is deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// User is active
        /// </summary>
        public bool IsActive { get; set; }

        /// Get or set latest logged-in
        /// </summary>
        public DateTime LatestLoggedin { get; set; }

        private ICollection<UserRole> _userRole;

        /// <summary>
        /// User roles
        /// </summary>
        public virtual ICollection<UserRole> UserRoles
        {
            get { return _userRole ?? (_userRole = new List<UserRole>()); }
            set { _userRole = value; }
        }

    }


    /// <summary>
    /// Gender enum
    /// </summary>
    public enum Gender
    {
        Female = 0,
        Male = 1,
        Other = 2,
    }
}
