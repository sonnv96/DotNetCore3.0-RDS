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
          IHostEnvironment  env,
          IMapper mapper,
          IUserService userService
         ) : base(config, env)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(User))]
        public async Task<ActionResult> GetAllUser(int pageIndex = 1, int pageSize = int.MaxValue)
        {
            var product = new List<Response>();
            var product2 = new List<Response2>();

            var a = new Response
            {
                Id = 1,
                FirstName = "abc"
            };

            product.Add(a);
            var query = _userService.FilterUserListByOptions();
        
            var users = await PagingHelper.Page(
                query,
                pageIndex, pageSize,
                x => x
                );
            _mapper.Map(users.Items, product);


            return ResponseHelper.Ok(users, "Succes");
        }


        //public async Task<IActionResult> GetAllUser()
        //{
        //    //var error = await CheckPermission(Permission.ListMerchant);
        //    //if (error != null)
        //    //    return ResponseHelper.BadRequest(error);

        //    // get list merchant with role
        //    var merchants = _userService.GetUserListByOptions();

        //    return ResponseHelper.Ok(merchants, "Đm succes");
        //}
    }
}