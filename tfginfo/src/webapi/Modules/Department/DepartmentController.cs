using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/department")]
[ApiController]
public class DepartmentController : BaseController
{
    public DepartmentController(ApplicationDbContext context) : base(context) { }


    [HttpPost]
    public IActionResult Save([FromBody] DepartmentFlatDTO department)
    {
        try
        {
            DepartmentManager manager = new DepartmentManager(context);
            return Ok(manager.CreateDepartment(department));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        DepartmentManager manager = new DepartmentManager(context);
        return Ok(manager.GetAllDepartments());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            DepartmentManager manager = new DepartmentManager(context);
            manager.DeleteDepartment(id);
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
    public IActionResult Update([FromBody] DepartmentFlatDTO department)
    {
        try
        {
            DepartmentManager manager = new DepartmentManager(context);
            return Ok(manager.UpdateDepartment(department));
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

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            DepartmentManager manager = new DepartmentManager(context);
            return Ok(manager.GetDepartment(id));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }


    [HttpGet("university/{universityId}")]
    public IActionResult GetAllByUniversity(int universityId)
    {
        DepartmentManager manager = new DepartmentManager(context);
        return Ok(manager.GetDepartmentsByUniversity(universityId));
    }

    [HttpPost("search")]
    public IActionResult Search([FromBody] List<Filter> filters)
    {
        DepartmentManager manager = new DepartmentManager(context);
        return Ok(manager.SearchDepartments(filters));
    }
}
