using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RDS.API.Features.Tokens
{
    public class RefreshTokenCommand
    {
        public class Request : IValidatableObject
        {
            /// <summary>
            /// Refresh Token
            /// </summary>
            [Required]
            public string RefreshToken { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                yield break;
            }
        }

        public class RequestExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return new Request
                {
                    RefreshToken = "adadase421123213",
                };
            }
        }

        public class Response
        {
            /// <summary>
            /// this is new Access Token
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

        public class OkResponseExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return new Response
                {
                    AccessToken = "asdas87987912asd",
                    RefreshToken = "asda23123123sdf",
                    AccessTokenExpiredAt = DateTime.Now.AddMinutes(60).ToString("o"),
                    RefreshTokenExpiredAt = DateTime.Now.AddDays(30).ToString("o"),
                };
            }
        }
    }
}
