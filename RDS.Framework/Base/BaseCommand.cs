using Microsoft.AspNetCore.Http;
using RDS.Framework.Helpers;
using Swashbuckle.AspNetCore.Filters;
//using Swashbuckle.AspNetCore.Examples;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDS.Framework.Base
{
    public class BaseCommand
    {
        public class BadRequestResponseExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return ResponseHelper.FailJson(StatusCodes.Status400BadRequest.ToString(), "Invalid this field");
            }
        }

        public class UnauthorizedResponseExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return ResponseHelper.FailJson(StatusCodes.Status401Unauthorized.ToString(), "Unauthorized");
            }
        }

        public class NotFoundResponseExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return ResponseHelper.FailJson(StatusCodes.Status404NotFound.ToString(), "NotFound");
            }
        }

        public class OkResponseExample : IExamplesProvider
        {
            public object GetExamples()
            {
                return ResponseHelper.SuccessJson(new { }, "success");
            }
        }

        public class OkEnumListResponseExample<TEnum> : IExamplesProvider where TEnum : Enum
        {
            public object GetExamples()
            {
                var lst = EnumHelper.GetJsons<TEnum>();
                return ResponseHelper.SuccessJson(lst, "success");
            }
        }
    }
}
