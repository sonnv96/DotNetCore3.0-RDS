using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RDS.API.Features.Base;
using RDS.API.Models.Users;
using RDS.Core.Entities;
using RDS.Core.Entities.Users;
using RDS.Framework.Helpers;
using RDS.Framework.Services.Users;
using Swashbuckle.AspNetCore.Filters;

namespace RDS.API.Features.Users
{
    [Route("api/[controller]")]

    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(
          IConfiguration config,
          IHostEnvironment env,
          IMapper mapper,
          IUserService userService
         ) : base(config, env)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(User))]
        public IActionResult List(int pageIndex = 1, int pageSize = int.MaxValue)
        {
            var res = new UserListModel();

            try
            {
                var query = _userService.Search();


                //_mapper.Map(users.Items, res.Users);
                //res.TotalItems = users.TotalItems;



                return ResponseHelper.Ok(query, "Success");

            }
            catch (Exception ex)
            {
                res.ErrorMessages.Add(ex.Message);
                return ResponseHelper.BadRequest(res.ErrorMessages);
            }


        }

        [HttpGet("detail/{id}")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(User))]
        public IActionResult Detail(int id)
        {
            var res = new UserListModel();
            try
            {
                //get user by identity
                var user = _userService.GetById(id);

                if (user == null)
                {
                    //not create before
                    return ResponseHelper.BadRequest(res.ErrorMessages);
                }

                //user.UserRoles = user.UserRoles.Where(x => x.IsActive && !x.Deleted).ToList();


                return ResponseHelper.Ok(user, "Success");

            }
            catch (Exception ex)
            {
                res.ErrorMessages.Add(ex.Message);
                return ResponseHelper.BadRequest(res.ErrorMessages);
            }
        }


        //[HttpGet("all")]
        //[SwaggerResponseExample(StatusCodes.Status200OK, typeof(User))]
        //public async Task<ActionResult> Add([FromBody] UserAddModel model)
        //{
        //    var res = new ApiJsonHelper();

        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            res.ErrorMessages.AddRange(ModelState.ToListErrorMessage());
        //            return ResponseHelper.BadRequest(res.ErrorMessages);
        //        }

        //        var exist = await _userService.CheckExistAsync(s => s.Username == model.Username);
        //        if (exist)
        //        {
        //            res.ErrorMessages.Add("User is exited");
        //            return ResponseHelper.BadRequest(res.ErrorMessages);
        //        }

        //        if (!string.IsNullOrEmpty(model.Email) && await _userService.CheckExistAsync(s => s.Email == model.Email))
        //        {
        //            res.ErrorMessages.Add("User.EmailInUsedByAnother");
        //            return ResponseHelper.BadRequest(res.ErrorMessages);
        //        }


        //        ////Add user role
        //        //if (model.Roles != null && model.Roles.Any())
        //        //{
        //        //    //get list user role identity
        //        //    var ids = model.Roles.Select(x => x.Id).ToList();
        //        //    //Get list user role from database
        //        //    var userRoles = _userRoleService.GetByIds(ids);

        //        //    // Check valid user roles input
        //        //    if (userRoles.Count() != ids.Count)
        //        //    {
        //        //        res.ErrorMessages.Add("User.InvalidRoles");
        //        //        return ResponseHelper.BadRequest(res.ErrorMessages);
        //        //    }

        //        //    var rolesInactive = userRoles.Where(x => !x.IsActive || x.Deleted);
        //        //    if (rolesInactive.Any())
        //        //    {
        //        //        res.ErrorMessages.Add(string.Join(",", rolesInactive.Select(x => x.Name)));
        //        //        res.ErrorMessages.Add("User.InactiveOrDeletedRoles");
        //        //        return ResponseHelper.BadRequest(res.ErrorMessages);
        //        //    }

        //        //    // set roles for user
        //        //    Mapper.Map(userRoles, entity.UserRoles);
        //        //}
        //        //Create entity
        //        var entity = _mapper.Map<UserAddModel, User>(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.ErrorMessages.Add(ex.Message);
        //        return ResponseHelper.BadRequest(res.ErrorMessages);
        //    }

        //}


    }
}