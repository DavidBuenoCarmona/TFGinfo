using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/working-group")]
[ApiController]
public class WorkingGroupController : BaseController
{
    private readonly EmailService emailService;
    public WorkingGroupController(ApplicationDbContext context, EmailService emailService, IConfiguration configuration) : base(context, configuration)
    {
        this.emailService = emailService;
    }

    /// <summary>
    ///   Crea un nuevo canal.
    /// Requiere rol de administrador o profesor.
    /// </summary>
    /// <param name="WorkingGroup">Datos del canal a crear.</param>
    /// <returns>Datos del nuevo canal.</returns>
    [HttpPost]
    public IActionResult Save([FromBody] WorkingGroupProfessor WorkingGroup)
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

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.CreateWorkingGroup(WorkingGroup.working_group, [WorkingGroup.professor]));
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
    ///  Obtiene todos los canales.
    /// Requiere autenticación.
    /// </summary>
    /// <returns>Lista de todos los canales.</returns>
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

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetAllWorkingGroups());
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
    /// Elimina un canal por su ID.
    /// Requiere rol de administrador o profesor.
    /// </summary>
    /// <param name="id">ID del canal a eliminar.</param>
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
            authManager.ValidateRoles(token, new List<int> { (int)RoleTypes.Admin, (int)RoleTypes.Professor });

            WorkingGroupManager manager = new WorkingGroupManager(context);
            manager.DeleteWorkingGroup(id);
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
    /// Actualiza un canal existente.
    /// Requiere rol de administrador o profesor.
    /// </summary>
    /// <param name="WorkingGroup">Datos del canal a actualizar.</param>
    /// <returns>canal actualizado.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] WorkingGroupBase WorkingGroup)
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

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.UpdateWorkingGroup(WorkingGroup));
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
    /// Obtiene un canal por su ID.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="id">ID del canal.</param>
    /// <returns>El canal solicitado.</returns>
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

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetWorkingGroup(id));
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
    /// Obtiene todos los profesores de un canal.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="id">ID del canal.</param>
    /// <returns>Lista de profesores del canal.</returns>
    [HttpGet("{id}/professor")]
    public IActionResult GetProfessorsByWorkingGroup(int id)
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

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetProfessorsByWorkingGroup(id));
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
    /// Obtiene todos los estudiantes de un canal.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="id">ID del canal.</param>
    /// <returns>Lista de estudiantes del canal.</returns>
    [HttpGet("{id}/student/")]
    public IActionResult GetStudentsByWorkingGroup(int id)
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

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetStudentsByWorkingGroup(id));
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
    /// Obtiene todos los TFGs de un canal.
    /// Requiere autenticación.
    /// </summary>
    /// <param name="id">ID del canal.</param>
    /// <returns>Lista de TFGs del canal.</returns>
    [HttpGet("{id}/tfg")]
    public IActionResult GetTFGsByWorkingGroup(int id)
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

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetTFGsByWorkingGroup(id));
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
    /// Obtiene todos los canales asociados a un profesor.
    /// Requiere rol de administrador o profesor.
    /// </summary>
    /// <param name="id">ID del profesor.</param>
    /// <returns>Lista de canales del profesor.</returns>
    [HttpGet("professor/{id}")]
    public IActionResult GetWorkingGroupByProfessor(int id)
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
            // Validate the professor's ID
            if (user.role.id != (int)RoleTypes.Admin && user.id != id)
            {
                return Unauthorized("You are not authorized to access this resource.");
            }

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetWorkingGroupsByProfessor(id));
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
    /// Obtiene todos los canales asociados a un estudiante.
    /// Requiere rol de administrador o estudiante.
    /// </summary>
    /// <param name="id">ID del estudiante.</param>
    /// <returns>Lista de canales del estudiante.</returns>
    [HttpGet("student/{id}")]
    public IActionResult GetWorkingGroupByStudent(int id)
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
            // Validate the professor's ID
            if (user.role.id != (int)RoleTypes.Admin && user.id != id)
            {
                return Unauthorized("You are not authorized to access this resource.");
            }

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetWorkingGroupsByStudent(id));
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
    /// Agrega un estudiante a un canal.
    /// Requiere rol de administrador o estudiante.
    /// </summary>
    /// <param name="workingGroupStudent">Objeto que contiene el ID del canal y el ID del estudiante.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpPost("add-student")]
    public IActionResult AddStudent([FromBody] WorkingGroupUser workingGroupStudent)
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
            if (user.role.id != (int)RoleTypes.Admin && user.id != workingGroupStudent.user)
            {
                return Unauthorized("You are not authorized to add students to this working group.");
            }
            WorkingGroupManager manager = new WorkingGroupManager(context);
            manager.AddStudentToWorkingGroup(workingGroupStudent.working_group, workingGroupStudent.user);
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
    /// Agrega un estudiante a un canal mediante su correo electrónico.
    /// Requiere rol de administrador o profesor.
    /// </summary>
    /// <param name="id">ID del canal.</param>
    /// <param name="email">Correo electrónico del estudiante a agregar.</param>
    [HttpPost("{id}/add-student/{email}")]
    public IActionResult AddStudentFromEmail(int id, string email)
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

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.AddStudentToWorkingGroupByEmail(id, email));
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
    /// Elimina un estudiante de un canal.
    /// Requiere rol de administrador o estudiante.
    /// </summary>
    /// <param name="workingGroupStudent">Objeto que contiene el ID del canal y el ID del estudiante.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpPost("remove-student")]
    public IActionResult RemoveStudent([FromBody] WorkingGroupUser workingGroupStudent)
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
            if (user.role.id == (int)RoleTypes.Student && user.id != workingGroupStudent.user)
            {
                return Unauthorized("You are not authorized to remove students from this working group.");
            }

            WorkingGroupManager manager = new WorkingGroupManager(context);
            manager.RemoveStudentFromWorkingGroup(workingGroupStudent.working_group, workingGroupStudent.user);
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
    /// Agrega un profesor a un canal.
    /// Requiere rol de administrador o profesor.
    /// </summary>
    /// <param name="workingGroupProfessor">Objeto que contiene el ID del canal y el ID del profesor.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpPost("add-professor")]
    public IActionResult AddProfessor([FromBody] WorkingGroupUser workingGroupProfessor)
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
            // Validate the professor's ID
            if (user.role.id != (int)RoleTypes.Admin && user.id != workingGroupProfessor.user)
            {
                return Unauthorized("You are not authorized to add professors to this working group.");
            }

            WorkingGroupManager manager = new WorkingGroupManager(context);
            manager.AddProfessorToWorkingGroup(workingGroupProfessor.working_group, workingGroupProfessor.user);
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
    /// Elimina un profesor de un canal.
    /// Requiere rol de administrador o profesor.
    /// </summary>
    /// <param name="workingGroupProfessor">Objeto que contiene el ID del canal y el ID del profesor.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpPost("remove-professor")]
    public IActionResult RemoveProfessor([FromBody] WorkingGroupUser workingGroupProfessor)
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
            // Validate the professor's ID
            if (user.role.id != (int)RoleTypes.Admin && user.id != workingGroupProfessor.user)
            {
                return Unauthorized("You are not authorized to remove professors from this working group.");
            }
            WorkingGroupManager manager = new WorkingGroupManager(context);
            manager.RemoveProfessorFromWorkingGroup(workingGroupProfessor.working_group, workingGroupProfessor.user);
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
    }

    /// <summary>
    /// Envía un mensaje a todos los estudiantes de un canal.
    /// </summary>
    /// <param name="message">Objeto que contiene el ID del canal, el ID del profesor y el mensaje a enviar.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] WorkingGroupMessage message)
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
            // Validate the professor's ID
            if (user.role.id != (int)RoleTypes.Admin && user.id != message.professor)
            {
                return Unauthorized("You are not authorized to send messages for this working group.");
            }

            WorkingGroupManager manager = new WorkingGroupManager(context, emailService);
            await manager.SendMessage(message.working_group, message.professor, message.message);
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
    /// Busca canales según filtros especificados.
    /// </summary>
    /// <param name="filters">Lista de filtros a aplicar en la búsqueda.</param>
    /// <returns>Resultado de la operación.</returns>
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

            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.SearchWorkingGroups(filters));
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
