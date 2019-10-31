using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RDS.Framework.Helpers
{
    public static class TokenHelper
    {
        public static string CreateJwtToken(int jwtExpMins, string jwtKey, string uniqueName
            /*(int csn, string playerId, VinID.Shared.Entities.ClientType clientType)? mobileInfo = null*/)
        {
            if (uniqueName == null)
                throw new Exception("uniqueName can not be null");

            var now = DateTime.UtcNow;
            var nowDateTimeOffset = new DateTimeOffset(now);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName),
                //new Claim(JwtRegisteredClaimNames.Sub, uniqueName),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Iat, nowDateTimeOffset.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };
            //if (mobileInfo != null)
            //{
            //    claims.Add(new Claim(SoapClaimNames.CSN, mobileInfo.Value.csn.ToString()));
            //    claims.Add(new Claim(SoapClaimNames.PLAYER_ID, mobileInfo.Value.playerId));
            //    claims.Add(new Claim(SoapClaimNames.CLIENT_TYPE, mobileInfo.Value.clientType.ToString()));
            //}

            var jwt = new JwtSecurityToken(
                //issuer: _configuration["BearerJwt:JwtIssuer"],
                //audience: _configuration["BearerJwt:JwtAudience"],
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(jwtExpMins),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static ClaimsPrincipal GetClaimsPrincipal(string token, string jwtKey)
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = false,
                ValidateActor = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateTokenReplay = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey))
            };

            SecurityToken securityToken = new JwtSecurityToken();
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, validationParameters, out securityToken);
        }
    }
}
