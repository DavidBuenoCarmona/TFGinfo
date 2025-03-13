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
        public BaseController(ApplicationDbContext context)
        {
            this.context = context;
        }
    }
}
