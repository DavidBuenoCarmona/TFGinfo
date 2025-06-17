using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/tfg-line")]
[ApiController]
public class TFGLineController : BaseController
{
    public TFGLineController(ApplicationDbContext context, IConfiguration configuration) : base(context, configuration){}


    [HttpPost]
    public IActionResult Save([FromBody] TFGLineFlatDTO TFGLine)
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
            AppUserDTO user = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });

            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.CreateTFGLine(TFGLine));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            // Log the exception (not shown here for brevity)
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
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
            AppUserDTO user = authManager.ValidateRoles(token, []);

            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.GetAllTFGLines());
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            // Log the exception (not shown here for brevity)
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
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
            AppUserDTO user = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });

            TFGLineManager manager = new TFGLineManager(context);
            manager.DeleteTFGLine(id);
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
        catch (Exception e)
        {
            // Log the exception (not shown here for brevity)
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
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
            AppUserDTO user = authManager.ValidateRoles(token, []);

            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.SearchTFGLines(filters));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            // Log the exception (not shown here for brevity)
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] TFGLineFlatDTO TFGLine)
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
            AppUserDTO user = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });

            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.UpdateTFGLine(TFGLine));
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
        catch (Exception e)
        {
            // Log the exception (not shown here for brevity)
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }

    [HttpPost("add-career/{id}")]
    public IActionResult AddCareer(int id, [FromBody] List<int> careers)
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
            AppUserDTO user = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });

            TFGLineManager manager = new TFGLineManager(context);
            manager.AddCareers(id, careers);
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
        catch (Exception e)
        {
            // Log the exception (not shown here for brevity)
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }

    [HttpPost("add-professor/{id}")]
    public IActionResult AddProfessor(int id, [FromBody] List<int> professors)
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
            AppUserDTO user = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });

            TFGLineManager manager = new TFGLineManager(context);
            manager.AddProfessors(id, professors);
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
        catch (Exception e)
        {
            // Log the exception (not shown here for brevity)
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
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
            AppUserDTO user = authManager.ValidateRoles(token, []);

            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.GetTFGLine(id));
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
        catch (Exception e)
        {
            // Log the exception (not shown here for brevity)
            return StatusCode(500, "An unexpected error occurred: " + e.Message);
        }
    }

    [HttpGet("student/{id}")]
    public IActionResult GetByStudentId(int id)
    {
        try {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            AppUserDTO user = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin, (int)RoleTypes.Student });
            if (user.role.id != (int)RoleTypes.Admin && user.id != id)
            {
                return Unauthorized("You do not have permission to access this resource.");
            }

            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.GetByStudentId(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet("professor/{id}")]
    public IActionResult GetByProfessorId(int id)
    {
        try {
            string token = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid or missing authorization token.");
            }
            token = token.Substring("Bearer ".Length).Trim();
            AuthManager authManager = new AuthManager(context, configuration);
            AppUserDTO user = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin, (int)RoleTypes.Professor });
            if (user.role.id != (int)RoleTypes.Admin && user.id != id)
            {
                return Unauthorized("You do not have permission to access this resource.");
            }

            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.GetByProfessorId(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

}
