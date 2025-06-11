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
    public readonly EmailService emailService;
    public StudentController(ApplicationDbContext context, EmailService emailService) : base(context)
    {
        this.emailService = emailService;
    }
    

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] StudentFlatDTO Student)
    {
        try {
            StudentManager manager = new StudentManager(context, emailService);
            var result = await manager.CreateStudent(Student);
            return Ok(result);
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

    [HttpPost("search")]
    public IActionResult Search([FromBody] List<Filter> filters)
    {
        StudentManager manager = new StudentManager(context);
        return Ok(manager.SearchStudents(filters));
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

    [HttpPut("{id}/optional-data")]
    public IActionResult UpdateOptionalData(int id, [FromBody] StudentOptionalDataDTO optionalData)
    {
        try {
            StudentManager manager = new StudentManager(context);
            return Ok(manager.UpdateOptionalData(id, optionalData));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id) {
        try {
            StudentManager manager = new StudentManager(context);
            return Ok(manager.GetById(id));
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
