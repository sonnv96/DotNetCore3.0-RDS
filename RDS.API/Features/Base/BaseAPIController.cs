using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RDS.Framework.Base;

namespace RDS.API.Features.Base
{
    public class BaseApiController : BaseController
    {
        public readonly IConfiguration _config;
        public readonly IHostEnvironment _env;
        //public readonly IHostingEnvironment _env;
        public BaseApiController(
           IConfiguration config,
           //IHostingEnvironment env
           IHostEnvironment env)
        {
            _config = config;
            _env = env;
        }
    }
}