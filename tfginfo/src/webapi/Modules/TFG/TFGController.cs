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
    public TFGController(ApplicationDbContext context, EmailService emailService, IConfiguration configuration) : base(context, configuration)
    {
        this.emailService = emailService;
    }

    /// <summary>
    /// Crea un nuevo TFG.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="TFG">Datos del TFG a crear.</param>
    /// <returns>TFG creado.</returns>
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

    /// <summary>
    /// Obtiene todos los TFGs.
    /// Requiere autenticación.
    /// </summary>
    /// <returns>Lista de TFGs.</returns>
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

     /// <summary>
    /// Busca TFGs según filtros.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="filters">Lista de filtros para la búsqueda.</param>
    /// <returns>Lista de TFGs encontrados.</returns>
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

    /// <summary>
    /// Elimina un TFG por su ID.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="id">ID del TFG a eliminar.</param>
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

    /// <summary>
    /// Actualiza un TFG existente.
    /// Requiere rol de administrador.
    /// </summary>
    /// <param name="TFG">Datos del TFG a actualizar.</param>
    /// <returns>TFG actualizado.</returns>
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

    /// <summary>
    /// Obtiene un TFG por su ID.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="id">ID del TFG a obtener.</param>
    /// <returns>TFG encontrado.</returns>
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
    
    /// <summary>
    /// Permite a un estudiante solicitar un TFG.
    /// </summary>
    /// <param name="request">Datos de la solicitud de TFG.</param>
    /// <returns>Resultado de la operación.</returns>
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

    /// <summary>
    /// Obtiene las solicitudes de TFG pendientes para un profesor.
    /// Requiere rol de profesor o administrador.
    /// </summary>
    /// <param name="id">ID del profesor.</param>
    /// <returns>Lista de solicitudes de TFG pendientes.</returns>
    [HttpGet("professor-pending/{id}")]
    public IActionResult GetTFGsByProfessor(int id)
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
            return Ok(manager.GetTFGsByProfessor(id));
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
    /// Acepta una solicitud de TFG.
    /// Requiere rol de profesor o administrador.
    /// </summary>
    /// <param name="id">ID del TFG a aceptar.</param>
    /// <returns>Resultado de la operación.</returns>
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

    /// <summary>
    /// Rechaza una solicitud de TFG.
    /// Requiere rol de profesor o administrador.
    /// </summary>
    /// <param name="id">ID del TFG a rechazar.</param>
    /// <returns>Resultado de la operación.</returns>
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
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }

    /// <summary>
    /// Cambia el estado de un TFG.
    /// Requiere rol de profesor o administrador.
    /// </summary>
    /// <param name="id">ID del TFG a cambiar de estado.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpPost("change-status/{id}")]
    public async Task<IActionResult> ChangeStatus(int id)
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
            await manager.ChangeStatus(id);
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
}
