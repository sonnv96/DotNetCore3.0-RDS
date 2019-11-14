using AutoMapper;
using RDS.Core.Entities;
using RDS.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.API.Models.Users
{
    public class UserListModel : ApiJsonHelper
    {
        public UserListModel()
        {
            Users = new List<UserModel>();
            TotalItems = 0;
        }

        /// <summary>
        /// List of user roles
        /// </summary>
        public List<UserModel> Users { get; set; }
        /// <summary>
        /// get or set total
        /// </summary>
        public int TotalItems { get; set; }
    }

    public class UserModel
    {
        /// <summary>
        /// User identify
        /// </summary>
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
        /// Full name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Get or set isActive
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Get or set latest logged-in
        /// </summary>
        public DateTime LatestLoggedin { get; set; }
        /// <summary>
        /// User roles
        /// </summary>
        //public List<UserRoleModel> UserRoles { get; set; }
    }

    public class UserAddModel
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Full name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Phone number
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Gender identity : 1-Male; 2-Female
        /// </summary>
        public int GenderId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// date of birth
        /// </summary>
        public DateTime? BirthDate { get; set; }
        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// User roles
        /// </summary>
        //public List<UserRoleEditModel> UserRoles { get; set; }
        public List<UserModel> Roles { get; set; }
    }

}
