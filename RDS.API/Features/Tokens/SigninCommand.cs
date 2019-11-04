using RDS.Framework.Helpers;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.API.Features.Tokens
{
    public class SigninCommand
    {
        public class RequestSignin : IValidatableObject
        {
            /// <summary>
            /// This is UserName
            /// </summary>
            [Required]
            public string Username { get; set; }

            /// <summary>
            /// This is Password
            /// </summary>
            [Required]
            public string Password { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                yield break;
            }
        }

        public class Response
        {
            /// <summary>
            /// This is AccessToken will be used for other APIs later
            /// </summary>
            public string AccessToken { get; set; }

            /// <summary>
            /// This is ExpiredAt of AccessToken
            /// </summary>
            public string AccessTokenExpiredAt { get; set; }

            /// <summary>
            /// This is RefreshToken
            /// </summary>
            public string RefreshToken { get; set; }

            /// <summary>
            /// This is ExpiredAt of RefreshToken
            /// </summary>
            public string RefreshTokenExpiredAt { get; set; }
        }

        public class RequestExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return new RequestSignin
                {
                    Username = "admin",
                    Password = "123456"
                };
            }
        }

        public class OkResponseExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return ResponseHelper.SuccessJson(new Response
                {
                    AccessToken = "asdas87987912asd",
                    RefreshToken = "asda23123123sdf",
                    AccessTokenExpiredAt = DateTime.Now.AddMinutes(60).ToString("o"),
                    RefreshTokenExpiredAt = DateTime.Now.AddDays(30).ToString("o"),
                }, "success");
            }
        }

    }
}
