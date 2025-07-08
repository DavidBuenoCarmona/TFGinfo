using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/professor")]
[ApiController]
public class ProfessorController : BaseController
{
    private readonly EmailService emailService;
    public ProfessorController(ApplicationDbContext context, EmailService emailService, IConfiguration configuration) : base(context, configuration)
    {
        this.emailService = emailService;
    }


    [HttpPost]
    public async Task<IActionResult> Save([FromBody] ProfessorFlatDTO Professor)
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

            ProfessorManager manager = new ProfessorManager(context, emailService);
            var result = await manager.CreateProfessor(Professor);
            return Ok(result);
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
            ProfessorManager manager = new ProfessorManager(context);
            return Ok(manager.GetAllProfessors());
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

            ProfessorManager manager = new ProfessorManager(context);
            manager.DeleteProfessor(id);
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
    public IActionResult Update([FromBody] ProfessorFlatDTO Professor)
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

            ProfessorManager manager = new ProfessorManager(context);
            return Ok(manager.UpdateProfessor(Professor));
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
    public IActionResult GetProfessorById(int id)
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

            ProfessorManager manager = new ProfessorManager(context);
            return Ok(manager.GetProfessorById(id));
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

            ProfessorManager manager = new ProfessorManager(context);
            return Ok(manager.SearchProfessors(filters));
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

    [HttpPost("import")]
    public async Task<IActionResult> Import([FromBody] CSVImport input)
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

            ProfessorManager manager = new ProfessorManager(context, emailService);
            var result = await manager.ImportProfessors(input.content);
            return Ok(result);
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
}
