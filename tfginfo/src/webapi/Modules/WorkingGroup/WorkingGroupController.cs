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
    public WorkingGroupController(ApplicationDbContext context, EmailService emailService) : base(context) {
        this.emailService = emailService;
    }
    

    [HttpPost]
    public IActionResult Save([FromBody] WorkingGroupProfessor WorkingGroup)
    {
        try {
           WorkingGroupManager manager = new WorkingGroupManager(context);
           return Ok(manager.CreateWorkingGroup(WorkingGroup.working_group, [WorkingGroup.professor]));
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        WorkingGroupManager manager = new WorkingGroupManager(context);
        return Ok(manager.GetAllWorkingGroups());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            manager.DeleteWorkingGroup(id);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] WorkingGroupBase WorkingGroup)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.UpdateWorkingGroup(WorkingGroup));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetWorkingGroup(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet("{id}/professor")]
    public IActionResult GetProfessorsByWorkingGroup(int id)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetProfessorsByWorkingGroup(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet("{id}/student/")]
    public IActionResult GetStudentsByWorkingGroup(int id)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetStudentsByWorkingGroup(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet("{id}/tfg")]
    public IActionResult GetTFGsByWorkingGroup(int id)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetTFGsByWorkingGroup(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet("professor/{id}")]
    public IActionResult GetWorkingGroupByProfessor(int id)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetWorkingGroupsByProfessor(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }
    [HttpGet("student/{id}")]
    public IActionResult GetWorkingGroupByStudent(int id)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.GetWorkingGroupsByStudent(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPost("add-student")]
    public IActionResult AddStudent([FromBody] WorkingGroupUser workingGroupStudent)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            manager.AddStudentToWorkingGroup(workingGroupStudent.working_group, workingGroupStudent.user);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPost("{id}/add-student/{email}")]
    public IActionResult AddStudentFromEmail(int id, string email)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            return Ok(manager.AddStudentToWorkingGroupByEmail(id,  email));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPost("remove-student")]
    public IActionResult RemoveStudent([FromBody] WorkingGroupUser workingGroupStudent)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            manager.RemoveStudentFromWorkingGroup(workingGroupStudent.working_group, workingGroupStudent.user);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }
    [HttpPost("add-professor")]
    public IActionResult AddProfessor([FromBody] WorkingGroupUser workingGroupProfessor)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            manager.AddProfessorToWorkingGroup(workingGroupProfessor.working_group, workingGroupProfessor.user);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }
    [HttpPost("remove-professor")]
    public IActionResult RemoveProfessor([FromBody] WorkingGroupUser workingGroupProfessor)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context);
            manager.RemoveProfessorFromWorkingGroup(workingGroupProfessor.working_group, workingGroupProfessor.user);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] WorkingGroupMessage message)
    {
        try {
            WorkingGroupManager manager = new WorkingGroupManager(context, emailService);
            await manager.SendMessage(message.working_group, message.professor, message.message);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }
}
