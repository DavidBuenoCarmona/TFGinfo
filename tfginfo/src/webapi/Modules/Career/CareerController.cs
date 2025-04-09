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
    public CareerController(ApplicationDbContext context) : base(context) {}
    

    [HttpPost]
    public IActionResult Save([FromBody] CareerFlatDTO career)
    {
        try {
           CareerManager manager = new CareerManager(context);
           return Ok(manager.CreateCareer(career));
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        CareerManager manager = new CareerManager(context);
        return Ok(manager.GetAllCareers());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try {
            CareerManager manager = new CareerManager(context);
            manager.DeleteCareer(id);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] CareerFlatDTO career)
    {
        try {
            CareerManager manager = new CareerManager(context);
            return Ok(manager.UpdateCareer(career));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        CareerManager manager = new CareerManager(context);
        return Ok(manager.GetCareerById(id));
    }


    [HttpGet("university/{universityId}")]
    public IActionResult GetAllByUniversity(int universityId)
    {
        CareerManager manager = new CareerManager(context);
        return Ok(manager.GetCareersByUniversity(universityId));
    }
}
