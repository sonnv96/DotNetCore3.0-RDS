using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RDS.API.Features.Users;
using RDS.Core.Entities;
using RDS.Core.Entities.Tokens;
using RDS.Core.Entities.Users;
using RDS.Framework.Base;
using RDS.Framework.Helpers;
using RDS.Framework.Repositories;
using Swashbuckle.AspNetCore.Filters;
using static RDS.API.Features.Tokens.SigninCommand;

namespace RDS.API.Features.Tokens
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<BearerToken> _bearerTokenRepo;
        private readonly int jwtExpMins;
        private readonly int jwtRefExpMins;
        private readonly string jwtKey;

        public TokenController(
           IConfiguration config,
           IHostEnvironment env,
           IRepository<User> userRepo,
           IRepository<BearerToken> bearerTokenRepo)
        {
            _config = config;
            _env = env;
            _userRepo = userRepo;
            _bearerTokenRepo = bearerTokenRepo;
            jwtExpMins = Convert.ToInt32(_config["BearerJwt:JwtExpMins"]);
            jwtRefExpMins = Convert.ToInt32(_config["BearerJwt:JwtRefExpMins"]);
            jwtKey = _config["BearerJwt:JwtKey"];
        }


        /// <summary>
        ///  signin
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns token</response>
        /// <response code="400">If the request is invalid</response>   
        /// <response code="401">If the request is unauthorized</response>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerRequestExample(typeof(SigninCommand.RequestSignin), typeof(SigninCommand.RequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SigninCommand.OkResponseExample))]
        public async Task<IActionResult> Token([FromBody] SigninCommand.RequestSignin model)
        {
            // get user by user name
            var user = await _userRepo.Query().FirstOrDefaultAsync(x => x.Username == model.Username);
            if (user == null)
                return ResponseHelper.BadRequest("Wrong username or password");

            if (user.Deleted)
                return ResponseHelper.BadRequest("User has been blocked");

            // verify hashed password
            var result = PasswordHelper.VerifyHashedPassword(model.Password, user.Password, user.SaltPassword);
            if (!result)
                return ResponseHelper.BadRequest("Wrong username or password");

            var accessToken = TokenHelper.CreateJwtToken(jwtExpMins, jwtKey, model.Username);

            // generate refreshtoken
            var refTokenRecord = new BearerToken
            {
                UserId = user.Id,
                AccToken = accessToken,
                RefToken = Guid.NewGuid().ToString().Replace("-", "") + Guid.NewGuid().ToString().Replace("-", ""),
                RefTokenExpAt = DateTime.UtcNow.AddMinutes(jwtRefExpMins),
            };
            await _bearerTokenRepo.InsertAsync(refTokenRecord);


            return ResponseHelper.Ok(new SigninCommand.Response()
            {
                AccessToken = refTokenRecord.AccToken,
                AccessTokenExpiredAt = DateTime.UtcNow.AddMinutes(jwtExpMins).ToString("o"),
                RefreshToken = refTokenRecord.RefToken,
                RefreshTokenExpiredAt = refTokenRecord.RefTokenExpAt.ToString("o"),
                UserName = user.Username,
                UserId = user.Id.ToString()
            }); ;
        }

        /// <summary>
        /// Get new access-token using refresh-token
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Return new access-token</response>
        /// <response code="400">If the request is invalid</response>   
        /// <response code="401">If the request is unauthorized</response>
        //[Route("refresh")]

        [AllowAnonymous]
        [SwaggerRequestExample(typeof(RefreshTokenCommand.Request), typeof(RefreshTokenCommand.RequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RefreshTokenCommand.OkResponseExample))]
        [HttpGet]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand.Request model)
        {
            // check token & match with csn
            var bearerToken = await _bearerTokenRepo.Query()
                .FirstOrDefaultAsync(x => x.RefToken == model.RefreshToken);

            if (bearerToken == null)
                return ResponseHelper.BadRequest("Invalid RefreshToken");

            // get current user
            var user = await _userRepo.Query()
                .FirstOrDefaultAsync(x => x.Id == bearerToken.UserId);

            // check csn
            if (bearerToken.RefTokenExpAt < DateTime.UtcNow)
                return ResponseHelper.BadRequest("RefreshToken expired");

            // generate new accesstoken contain some user info
            var accessToken = TokenHelper.CreateJwtToken(jwtExpMins, jwtKey, user.Username);

            // update bearToken
            bearerToken.AccToken = accessToken;
            bearerToken.RefTokenExpAt = DateTime.UtcNow.AddMinutes(jwtRefExpMins);
            await _bearerTokenRepo.UpdateAsync(bearerToken);

            // return accesstoken
            return ResponseHelper.Ok(new RefreshTokenCommand.Response
            {
                AccessToken = bearerToken.AccToken,
                AccessTokenExpiredAt = DateTime.UtcNow.AddMinutes(jwtExpMins).ToString("o"),
                RefreshToken = bearerToken.RefToken,
                RefreshTokenExpiredAt = bearerToken.RefTokenExpAt.ToString("o"),
            });
        }
    }
}