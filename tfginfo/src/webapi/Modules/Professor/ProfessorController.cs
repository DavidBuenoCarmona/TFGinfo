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


    /// <summary>
    /// Crea un nuevo profesor.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="Professor">Datos del profesor a crear.</param>
    /// <returns>Datos del nuevo profesor y código de autenticación.</returns>
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

    /// <summary>
    /// Obtiene todos los profesores.
    /// Requiere autenticación.
    /// </summary>
    /// <returns>Lista de profesores.</returns>
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

    /// <summary>
    /// Elimina un profesor por su ID.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="id">ID del profesor a eliminar.</param>
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

    /// <summary>
    /// Actualiza un profesor existente.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="Professor">Datos del profesor a actualizar.</param>
    /// <returns>Profesor actualizado.</returns>
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

    /// <summary>
    /// Obtiene un profesor por su ID.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="id">ID del profesor.</param>
    /// <returns>El profesor solicitado.</returns>
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

    /// <summary>
    /// Busca profesores según filtros especificados.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="filters">Lista de filtros para la búsqueda.</param>
    /// <returns>Lista de profesores que coinciden con los filtros.</returns>
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

    /// <summary>
    /// Importa profesores desde un archivo CSV codificado en base64.
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
