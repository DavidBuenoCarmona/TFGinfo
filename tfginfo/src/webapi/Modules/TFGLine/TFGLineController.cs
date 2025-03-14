using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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


    [HttpGet("department/{departmentId}")]
    public IActionResult GetAllByDepartment(int departmentId)
    {
        TFGLineManager manager = new TFGLineManager(context);
        return Ok(manager.GetTFGLinesByDepartment(departmentId));
    }
}
