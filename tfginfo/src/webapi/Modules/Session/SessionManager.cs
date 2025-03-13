using System.Collections.Generic;
using TFGinfo.Data;

namespace TFGinfo.Api
{
    public class SessionManager : BaseManager
    {
        public SessionManager(ApplicationDbContext context) : base(context) {}

    }
}