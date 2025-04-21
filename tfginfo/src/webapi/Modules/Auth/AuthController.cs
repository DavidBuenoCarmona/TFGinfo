using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/auth")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IConfiguration configuration;
    public AuthController(ApplicationDbContext context, IConfiguration configuration) : base(context) {
        this.configuration = configuration;
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginCredentials credentails)
    {
        try {
           AuthManager manager = new AuthManager(context, configuration);
           return Ok(manager.Login(credentails));
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPost("change-password")]
    public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try {
            AuthManager manager = new AuthManager(context, configuration);
            return Ok(manager.ChangePassword(request));
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }
}
