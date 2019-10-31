using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RDS.API.Features.Base;
using RDS.Core.Entities;
using RDS.Framework.Helpers;
using RDS.Framework.Services.Users;
using Swashbuckle.AspNetCore.Filters;

namespace RDS.API.Features.Users
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(
          IConfiguration config,
          IHostEnvironment  env,
          IUserService userService
         ) : base(config, env)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(User))]
        public async Task<IActionResult> GetAllUser()
        {
            //var error = await CheckPermission(Permission.ListMerchant);
            //if (error != null)
            //    return ResponseHelper.BadRequest(error);

            // get list merchant with role
            var merchants = await _userService.GetUserListByOptions().ToListAsync();

            return ResponseHelper.Ok(merchants, "Đm succes");
        }
    }
}