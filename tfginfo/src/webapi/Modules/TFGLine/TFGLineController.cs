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
    public TFGLineController(ApplicationDbContext context, IConfiguration configuration) : base(context, configuration) { }

    /// <summary>
    /// Crea una nueva línea de TFG.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="TFGLine">Datos de la línea de TFG a crear.</param>
    /// <returns>La línea de TFG creada.</returns>
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

    /// <summary>
    /// Obtiene todas las líneas de TFG.
    /// Requiere autenticación.
    /// </summary>
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

    /// <summary>
    /// Elimina una línea de TFG por su ID.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="id">ID de la línea de TFG a eliminar.</param>
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

    /// <summary>
    /// Busca líneas de TFG según filtros proporcionados.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="filters">Lista de filtros para la búsqueda.</param>
    /// <returns>Lista de líneas de TFG encontradas.</returns>
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

    /// <summary>
    /// Actualiza una línea de TFG existente.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="TFGLine">Datos de la línea de TFG a actualizar.</param>
    /// <returns>TFGLine actualizado.</returns>
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

    /// <summary>
    /// Añade carreras a una línea de TFG.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="id">ID de la línea de TFG a actualizar.</param>
    /// <param name="careers">Lista de IDs de carreras a añadir.</param>
    /// <returns>Resultado de la operación.</returns>
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

    /// <summary>
    /// Añade profesores a una línea de TFG.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="id">ID de la línea de TFG a actualizar.</param>
    /// <param name="professors">Lista de IDs de profesores a añadir.</param
    /// <returns>Resultado de la operación.</returns>
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

    /// <summary>
    /// Obtiene una línea de TFG por su ID.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="id">ID de la línea de TFG.</param>
    /// <returns>La línea de TFG solicitada.</returns>
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

    /// <summary>
    /// Obtiene las líneas de TFG asociadas a un estudiante por su ID.
    /// Requiere rol de estudiante o administrador.
    /// </summary>
    /// <param name="id">ID del estudiante.</param>
    /// <returns>Lista de líneas de TFG asociadas al estudiante.</returns>
    [HttpGet("student/{id}")]
    public IActionResult GetByStudentId(int id)
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
            AppUserDTO user = authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin, (int)RoleTypes.Student });
            if (user.role.id != (int)RoleTypes.Admin && user.id != id)
            {
                return Unauthorized("You do not have permission to access this resource.");
            }

            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.GetByStudentId(id));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }

    /// <summary>
    /// Obtiene las líneas de TFG asociadas a un profesor por su ID.
    /// Requiere rol de profesor o administrador.
    /// </summary>
    /// <param name="id">ID del profesor.</param>
    /// <returns>Lista de líneas de TFG asociadas al profesor.</returns>
    [HttpGet("professor/{id}")]
    public IActionResult GetByProfessorId(int id)
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
            return Ok(manager.GetByProfessorId(id));
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
    
    /// <summary>
    /// Importa líneas de TFG desde un archivo CSV codificado en base64.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="input">Objeto que contiene el contenido del archivo CSV en base64
    /// </param>
    /// <returns>Resultado de la importación, incluyendo número de éxitos y errores.</returns>
    [HttpPost("import")]
    public IActionResult Import([FromBody] CSVImport input)
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

            TFGLineManager manager = new TFGLineManager(context);
            var result = manager.ImportTFGs(input.content);
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
