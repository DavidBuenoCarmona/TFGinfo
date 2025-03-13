using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Data;

[Route("/session")]
[ApiController]
public class SessionController : BaseController
{
    public SessionController(ApplicationDbContext context) : base(context) {}
    

    [HttpPost]
    public IActionResult login()
    {
        SessionManager sessionManager = new SessionManager(context);
        return Ok();
    }
}
