using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
//using Swashbuckle.AspNetCore.Examples;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDS.Framework.Base
{
    [Authorize]
    [ApiController]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BaseCommand.BadRequestResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(BaseCommand.UnauthorizedResponseExample))]
    public class BaseController : ControllerBase
    {
    }
}
