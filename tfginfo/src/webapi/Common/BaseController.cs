using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Data;

namespace TFGinfo.Api
{
    public class BaseController : ControllerBase
    {
        protected readonly ApplicationDbContext context;
        protected readonly IConfiguration configuration;
        public BaseController(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
    }
}
