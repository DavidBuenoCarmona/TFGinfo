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
    public TFGLineController(ApplicationDbContext context) : base(context) {}
    

    [HttpPost]
    public IActionResult Save([FromBody] TFGLineFlatDTO TFGLine)
    {
        try {
           TFGLineManager manager = new TFGLineManager(context);
           return Ok(manager.CreateTFGLine(TFGLine));
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        TFGLineManager manager = new TFGLineManager(context);
        return Ok(manager.GetAllTFGLines());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try {
            TFGLineManager manager = new TFGLineManager(context);
            manager.DeleteTFGLine(id);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPost("search")]
    public IActionResult Search([FromBody] List<Filter> filters)
    {
        try
        {
            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.SearchTFGLines(filters));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] TFGLineFlatDTO TFGLine)
    {
        try {
            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.UpdateTFGLine(TFGLine));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPost("add-career/{id}")]
    public IActionResult AddCareer(int id, [FromBody] List<int> careers)
    {
        try {
            TFGLineManager manager = new TFGLineManager(context);
            manager.AddCareers(id, careers);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPost("add-professor/{id}")]
    public IActionResult AddProfessor(int id, [FromBody] List<int> professors)
    {
        try {
            TFGLineManager manager = new TFGLineManager(context);
            manager.AddProfessors(id, professors);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet("department/{departmentId}")]
    public IActionResult GetAllByDepartment(int departmentId)
    {
        TFGLineManager manager = new TFGLineManager(context);
        return Ok(manager.GetTFGLinesByDepartment(departmentId));
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try {
            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.GetTFGLine(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet("student/{id}")]
    public IActionResult GetByStudentId(int id)
    {
        try {
            TFGLineManager manager = new TFGLineManager(context);
            return Ok(manager.GetByStudentId(id));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

}
