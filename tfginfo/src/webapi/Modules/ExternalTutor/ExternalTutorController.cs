using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/external-tutor")]
[ApiController]
public class ExternalTutorController : BaseController
{
    public ExternalTutorController(ApplicationDbContext context) : base(context) {}
    

    [HttpPost]
    public IActionResult Save([FromBody] ExternalTutorBase ExternalTutor)
    {
        try {
            ExternalTutorManager ExternalTutorManager = new ExternalTutorManager(context);
            return Ok(ExternalTutorManager.CreateExternalTutor(ExternalTutor));
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        ExternalTutorManager ExternalTutorManager = new ExternalTutorManager(context);
        return Ok(ExternalTutorManager.GetAllExternalTutors());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try {
            ExternalTutorManager ExternalTutorManager = new ExternalTutorManager(context);
            ExternalTutorManager.DeleteExternalTutor(id);
            return Ok();
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPut]
    public IActionResult Update([FromBody] ExternalTutorBase ExternalTutor)
    {
        try {
            ExternalTutorManager ExternalTutorManager = new ExternalTutorManager(context);
            return Ok(ExternalTutorManager.UpdateExternalTutor(ExternalTutor));
        } catch (NotFoundException) {
            return NotFound();
        } catch (UnprocessableException e) {
            return UnprocessableEntity(e.GetError());
        }
    }
}
