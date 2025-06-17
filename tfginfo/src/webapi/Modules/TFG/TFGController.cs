using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/tfg")]
[ApiController]
public class TFGController : BaseController
{   
    private readonly EmailService emailService;
    public TFGController(ApplicationDbContext context, EmailService emailService, IConfiguration configuration) : base(context, configuration) {
        this.emailService = emailService;
     }


    [HttpPost]
    public IActionResult Save([FromBody] TFGFlatDTO TFG)
    {
        try
        {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });

            TFGManager manager = new TFGManager(context);
            return Ok(manager.CreateTFG(TFG));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            authManager.ValidateRoles(token, []);

            TFGManager manager = new TFGManager(context);
            return Ok(manager.GetAllTFGs());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        
    }

    [HttpPost("search")]
    public IActionResult Search([FromBody] List<Filter> filters)
    {
        try
        {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            authManager.ValidateRoles(token, []);
            TFGManager manager = new TFGManager(context);
            return Ok(manager.SearchTFGs(filters));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });

            TFGManager manager = new TFGManager(context);
            manager.DeleteTFG(id);
            return Ok();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] TFGFlatDTO TFG)
    {
        try
        {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });

            TFGManager manager = new TFGManager(context);
            return Ok(manager.UpdateTFG(TFG));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            authManager.ValidateRoles(token, []);

            TFGManager manager = new TFGManager(context);
            return Ok(manager.GetTFGById(id));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPost("request")]
    public async Task<IActionResult> RequestTFG([FromBody] TFGRequest request)
    {
        try
        {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            AppUserDTO user = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Student });
            if (user.username != request.studentEmail)
            {
                return Unauthorized("You are not authorized to request this TFG.");
            }

            TFGManager manager = new TFGManager(context, emailService, configuration);
            await manager.RequestTFG(request);
            return Ok();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpGet("professor-pending/{id}")]
    public IActionResult GetPendingTFGsByProfessor(int id)
    {
        try
        {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            AppUserDTO user = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin, (int)RoleTypes.Professor });
            if (user.id != id && user.role.id != (int)RoleTypes.Admin)
            {
                return Unauthorized("You are not authorized to view pending TFGs for this professor.");
            }

            TFGManager manager = new TFGManager(context);
            return Ok(manager.GetPendingTFGsByProfessor(id));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPost("accept/{id}")]
    public async Task<IActionResult> Accept(int id)
    {
        try
        {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin, (int)RoleTypes.Professor });

            TFGManager manager = new TFGManager(context, emailService, configuration);
            await manager.AcceptTFG(id);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPost("reject/{id}")]
    public async Task<IActionResult> Reject(int id)
    {
        try
        {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin, (int)RoleTypes.Professor });

            TFGManager manager = new TFGManager(context, emailService, configuration);
            await manager.RejectTFG(id);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }
}
