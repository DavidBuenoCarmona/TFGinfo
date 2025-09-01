using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/university")]
[ApiController]
public class UniversityController : BaseController
{
    public UniversityController(ApplicationDbContext context, IConfiguration configuration) : base(context, configuration) { }


    /// <summary>
    /// Crea una nueva centro.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="university">Datos de la centro a crear.</param>
    /// <returns>Datos de la nueva centro.</returns>
    [HttpPost]
    public IActionResult Save([FromBody] UniversityBase university)
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

            UniversityManager universityManager = new UniversityManager(context);
            return Ok(universityManager.CreateUniversity(university));
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
    /// Obtiene todas las centros.
    /// Requiere autenticación.
    /// </summary>
    /// <returns>Lista de todas las centros.</returns>
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

            UniversityManager universityManager = new UniversityManager(context);
            return Ok(universityManager.GetAllUniversities());
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
    /// Elimina una centro por su ID.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="id">ID de la centro a eliminar.</param>
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

            UniversityManager universityManager = new UniversityManager(context);
            universityManager.DeleteUniversity(id);
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
    /// Actualiza una centro existente.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="university">Datos de la centro a actualizar.</param>
    /// <returns>centro actualizada.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] UniversityBase university)
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

            UniversityManager universityManager = new UniversityManager(context);
            return Ok(universityManager.UpdateUniversity(university));
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
    /// Obtiene una centro por su ID.
    /// Requiere autenticación.
    /// </summary>
    ///     <param name="id">ID de la centro.</param>
    /// <returns>La centro solicitada.</returns>
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
            authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });

            UniversityManager universityManager = new UniversityManager(context);
            return Ok(universityManager.GetUniversity(id));
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
    /// Busca centros según filtros especificados.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="filters">Lista de filtros para la búsqueda.</param>
    /// <returns>Lista de centros que coinciden con los filtros.</returns>
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

            UniversityManager universityManager = new UniversityManager(context);
            return Ok(universityManager.SearchUniversities(filters));
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
    ///     Importa centros desde un archivo CSV codificado en base64.
    ///   Requiere rol de administrador.
    /// </summary>
    /// <param name="input">Contenido del archivo CSV en base64.</param>
    /// <returns>Resultado de la importación con número de éxitos y errores.</returns>
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

            UniversityManager manager = new UniversityManager(context);
            var result = manager.ImportUniversities(input.content);
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
