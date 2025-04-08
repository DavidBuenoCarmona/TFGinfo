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
    public ProfessorController(ApplicationDbContext context) : base(context) {}
    

    [HttpPost]
    public IActionResult Save([FromBody] ProfessorFlatDTO Professor)
    {
        try {
           ProfessorManager manager = new ProfessorManager(context);
           return Ok(manager.CreateProfessor(Professor));
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        ProfessorManager manager = new ProfessorManager(context);
        return Ok(manager.GetAllProfessors());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try {
            ProfessorManager manager = new ProfessorManager(context);
            manager.DeleteProfessor(id);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] ProfessorFlatDTO Professor)
    {
        try {
            ProfessorManager manager = new ProfessorManager(context);
            return Ok(manager.UpdateProfessor(Professor));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }


    [HttpGet("department/{departmentId}")]
    public IActionResult GetAllByDepartment(int departmentId) {
        ProfessorManager manager = new ProfessorManager(context);
        return Ok(manager.GetAllByDepartment(departmentId));
    }

    [HttpGet("{id}")]
    public IActionResult GetProfessorById(int id)
    {
        try {
            ProfessorManager manager = new ProfessorManager(context);
            return Ok(manager.GetProfessorById(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }
}
