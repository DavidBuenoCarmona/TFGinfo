using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/career")]
[ApiController]
public class CareerController : BaseController
{
    public CareerController(ApplicationDbContext context, IConfiguration configuration) : base(context, configuration) { }

    /// <summary>
    /// Crea una nueva titulación.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="career">Datos de la titulación a crear.</param>
    /// <returns>Datos de la nueva titulación.</returns>
    [HttpPost]
    public IActionResult Save([FromBody] CareerFlatDTO career)
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

            CareerManager manager = new CareerManager(context);
            return Ok(manager.CreateCareer(career));
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
    /// Obtiene todas las titulaciones.
    /// Requiere rol de administrador.
    /// </summary>
    /// <returns>Lista de todos las titulaciones.</returns>
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

            CareerManager manager = new CareerManager(context);
            return Ok(manager.GetAllCareers());
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
    /// Elimina una titulación por su ID.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="id">ID de la titulación a eliminar.</param>
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

            CareerManager manager = new CareerManager(context);
            manager.DeleteCareer(id);
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
    /// Actualiza una titulación existente.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="career">Datos de la titulación a actualizar.</param>
    /// <returns>titulación actualizada.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] CareerFlatDTO career)
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

            CareerManager manager = new CareerManager(context);
            return Ok(manager.UpdateCareer(career));
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
    /// Obtiene una titulación por su ID.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="id">ID de la titulación.</param>
    /// <returns>La titulación solicitada.</returns>
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
            authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin });
            CareerManager manager = new CareerManager(context);
            return Ok(manager.GetCareerById(id));
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
    /// Busca titulaciones según filtros especificados.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="filters">Lista de filtros para la búsqueda.</param>
    /// <returns>Lista de titulaciones que coinciden con los filtros.</returns>
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

            CareerManager manager = new CareerManager(context);
            return Ok(manager.SearchCareers(filters));
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
    ///    Importa titulaciones desde un archivo CSV codificado en base64.
    ///  Requiere rol de administrador.
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

            CareerManager manager = new CareerManager(context);
            var result = manager.ImportCareers(input.content);
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
