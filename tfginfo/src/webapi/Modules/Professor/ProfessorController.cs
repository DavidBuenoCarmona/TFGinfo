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
    public ProfessorController(ApplicationDbContext context, EmailService emailService) : base(context)
    {
        this.emailService = emailService;
     }


    [HttpPost]
    public async Task<IActionResult> Save([FromBody] ProfessorFlatDTO Professor)
    {
        try
        {
            ProfessorManager manager = new ProfessorManager(context, emailService);
            var result = await manager.CreateProfessor(Professor);
            return Ok(result);
        }
        catch (UnprocessableException e)
        {
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
        try
        {
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
    }

    [HttpPut]
    public IActionResult Update([FromBody] ProfessorFlatDTO Professor)
    {
        try
        {
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
    }


    [HttpGet("department/{departmentId}")]
    public IActionResult GetAllByDepartment(int departmentId)
    {
        ProfessorManager manager = new ProfessorManager(context);
        return Ok(manager.GetAllByDepartment(departmentId));
    }

    [HttpGet("{id}")]
    public IActionResult GetProfessorById(int id)
    {
        try
        {
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
    }
    
    [HttpPost("search")]
    public IActionResult Search([FromBody] List<Filter> filters)
    {
        try
        {
            ProfessorManager manager = new ProfessorManager(context);
            return Ok(manager.SearchProfessors(filters));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }
}
