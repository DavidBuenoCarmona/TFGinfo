using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/university")]
[ApiController]
public class UniversityController : BaseController
{
    public UniversityController(ApplicationDbContext context) : base(context) { }


    [HttpPost]
    public IActionResult Save([FromBody] UniversityBase university)
    {
        try
        {
            UniversityManager universityManager = new UniversityManager(context);
            return Ok(universityManager.CreateUniversity(university));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        UniversityManager universityManager = new UniversityManager(context);
        return Ok(universityManager.GetAllUniversities());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            UniversityManager universityManager = new UniversityManager(context);
            universityManager.DeleteUniversity(id);
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
    public IActionResult Update([FromBody] UniversityBase university)
    {
        try
        {
            UniversityManager universityManager = new UniversityManager(context);
            return Ok(universityManager.UpdateUniversity(university));
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
    public IActionResult Get(int id)
    {
        try
        {
            UniversityManager universityManager = new UniversityManager(context);
            return Ok(universityManager.GetUniversity(id));
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
            UniversityManager universityManager = new UniversityManager(context);
            return Ok(universityManager.SearchUniversities(filters));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }
}
