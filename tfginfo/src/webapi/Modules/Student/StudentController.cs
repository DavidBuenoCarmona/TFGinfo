using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/student")]
[ApiController]
public class StudentController : BaseController
{
    public readonly EmailService emailService;
    public StudentController(ApplicationDbContext context, EmailService emailService, IConfiguration configuration) : base(context, configuration)
    {
        this.emailService = emailService;
    }


    [HttpPost]
    public async Task<IActionResult> Save([FromBody] StudentFlatDTO Student)
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

            StudentManager manager = new StudentManager(context, emailService);
            var result = await manager.CreateStudent(Student);
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
            authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });

            StudentManager manager = new StudentManager(context);
            return Ok(manager.GetAllStudents());
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
            authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });
            StudentManager manager = new StudentManager(context);
            return Ok(manager.SearchStudents(filters));
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

            StudentManager manager = new StudentManager(context);
            manager.DeleteStudent(id);
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
    public IActionResult Update([FromBody] StudentFlatDTO Student)
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

            StudentManager manager = new StudentManager(context);
            return Ok(manager.UpdateStudent(Student));
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

    [HttpPut("{id}/optional-data")]
    public IActionResult UpdateOptionalData(int id, [FromBody] StudentOptionalDataDTO optionalData)
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
            AppUserDTO User = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin, (int)RoleTypes.Student });

            if (User.id != id && User.role.id != (int)RoleTypes.Admin)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this student's optional data.");
            }
            StudentManager manager = new StudentManager(context);
            return Ok(manager.UpdateOptionalData(id, optionalData));

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
            AppUserDTO User = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin, (int)RoleTypes.Student });

            if (User.id != id && User.role.id != (int)RoleTypes.Admin)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this student's optional data.");
            }
            StudentManager manager = new StudentManager(context);
            return Ok(manager.GetById(id));
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
}
