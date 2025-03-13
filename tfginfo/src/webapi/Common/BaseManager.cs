using System.Collections.Generic;
using TFGinfo.Data;

namespace TFGinfo.Api
{
    public class BaseManager
    {
        protected readonly ApplicationDbContext context;

        public BaseManager(ApplicationDbContext context)
        {
            this.context = context;
        }
    }
}