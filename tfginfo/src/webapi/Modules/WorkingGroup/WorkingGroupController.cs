using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/working-group")]
[ApiController]
public class WorkingGroupController : BaseController
{
    public WorkingGroupController(ApplicationDbContext context) : base(context) {}
    

    [HttpPost]
    public IActionResult Save([FromBody] WorkingGroupBase WorkingGroup)
    {
        try {
           WorkingGroupManager manager = new WorkingGroupManager(context);
           return Ok(manager.CreateWorkingGroup(WorkingGroup));
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

    [HttpGet("{professor/id}")]
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

    [HttpGet("{student/id}")]
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

    [HttpGet("{tfg/{id}}")]
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
}
