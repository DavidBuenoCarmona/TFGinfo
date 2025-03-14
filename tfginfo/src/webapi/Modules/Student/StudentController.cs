using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/student")]
[ApiController]
public class StudentController : BaseController
{
    public StudentController(ApplicationDbContext context) : base(context) {}
    

    [HttpPost]
    public IActionResult Save([FromBody] StudentFlatDTO Student)
    {
        try {
           StudentManager manager = new StudentManager(context);
           return Ok(manager.CreateStudent(Student));
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        StudentManager manager = new StudentManager(context);
        return Ok(manager.GetAllStudents());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try {
            StudentManager manager = new StudentManager(context);
            manager.DeleteStudent(id);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] StudentFlatDTO Student)
    {
        try {
            StudentManager manager = new StudentManager(context);
            return Ok(manager.UpdateStudent(Student));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }


    [HttpGet("career/{careerId}")]
    public IActionResult GetAllByCareer(int careerId) {
        StudentManager manager = new StudentManager(context);
        return Ok(manager.GetAllByCareer(careerId));
    }
}
