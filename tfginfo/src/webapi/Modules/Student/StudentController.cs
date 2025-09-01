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

    /// <summary>
    /// Crea un nuevo estudiante.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="Student">Datos del estudiante a crear.</param>
    /// <returns>Datos del nuevo estudiante y código de autenticación.</returns>
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

    /// <summary>
    /// Importa estudiantes desde un archivo CSV codificado en base64.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="input">Objeto que contiene el contenido del archivo CSV en base64
    /// </param>
    /// <returns>Resultado de la importación, incluyendo número de éxitos y errores.</returns>
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

            StudentManager manager = new StudentManager(context, emailService);
            var result = await manager.ImportStudents(input.content);
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

    /// <summary>
    /// Obtiene todos los estudiantes.
    /// Requiere rol de administrador.
    /// </summary>
    /// <returns>Lista de todos los estudiantes.</returns>
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

    /// <summary>
    /// Busca estudiantes según filtros especificados.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="filters">Lista de filtros para la búsqueda.</param>
    /// <returns>Lista de estudiantes que coinciden con los filtros.</returns>
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

    /// <summary>
    /// Elimina un estudiante por su ID.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="id">ID del estudiante a eliminar.</param>
    /// <returns>Resultado de la operación.</returns>
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

    /// <summary>
    /// Actualiza un estudiante existente.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="Student">Datos del estudiante a actualizar.</param>
    /// <returns>Estudiante actualizado.</returns>
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

    /// <summary>
    /// Actualiza los datos opcionales de un estudiante.
    /// Requiere rol de administrador o el propio estudiante.
    /// </summary>
    /// <param name="id">ID del estudiante.</param>
    /// <param name="optionalData">Datos opcionales a actualizar.</param>
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
                throw new UnauthorizedAccessException("CANNOT_EDIT_THAT_STUDENT_INFORMATION");
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

    /// <summary>
    /// Obtiene un estudiante por su ID.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="id">ID del estudiante.</param>
    /// <returns>El estudiante solicitado.</returns>
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
                throw new UnauthorizedAccessException("CANNOT_EDIT_THAT_STUDENT_INFORMATION");
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
