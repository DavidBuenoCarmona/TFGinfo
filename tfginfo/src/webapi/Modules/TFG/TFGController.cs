using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/tfg")]
[ApiController]
public class TFGController : BaseController
{
    public TFGController(ApplicationDbContext context) : base(context) {}
    

    [HttpPost]
    public IActionResult Save([FromBody] TFGFlatDTO TFG)
    {
        try {
           TFGManager manager = new TFGManager(context);
           return Ok(manager.CreateTFG(TFG));
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        TFGManager manager = new TFGManager(context);
        return Ok(manager.GetAllTFGs());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try {
            TFGManager manager = new TFGManager(context);
            manager.DeleteTFG(id);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] TFGFlatDTO TFG)
    {
        try {
            TFGManager manager = new TFGManager(context);
            return Ok(manager.UpdateTFG(TFG));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }


    [HttpGet("tfgLine/{tfgLineId}")]
    public IActionResult GetTFGsByTFGLine(int tfgLineId)
    {
        TFGManager manager = new TFGManager(context);
        return Ok(manager.GetTFGsByTFGLine(tfgLineId));
    }
}
