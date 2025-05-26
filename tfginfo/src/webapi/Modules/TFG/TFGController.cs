using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFGinfo.Api;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Objects;

[Route("/tfg")]
[ApiController]
public class TFGController : BaseController
{   
    private readonly EmailService emailService;
    private readonly IConfiguration configuration;
    public TFGController(ApplicationDbContext context, EmailService emailService, IConfiguration configuration) : base(context) {
        this.emailService = emailService;
        this.configuration = configuration;
     }


    [HttpPost]
    public IActionResult Save([FromBody] TFGFlatDTO TFG)
    {
        try
        {
            TFGManager manager = new TFGManager(context);
            return Ok(manager.CreateTFG(TFG));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        TFGManager manager = new TFGManager(context);
        return Ok(manager.GetAllTFGs());
    }

    [HttpPost("search")]
    public IActionResult Search([FromBody] List<Filter> filters)
    {
        try
        {
            TFGManager manager = new TFGManager(context);
            return Ok(manager.SearchTFGs(filters));
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            TFGManager manager = new TFGManager(context);
            manager.DeleteTFG(id);
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
    public IActionResult Update([FromBody] TFGFlatDTO TFG)
    {
        try
        {
            TFGManager manager = new TFGManager(context);
            return Ok(manager.UpdateTFG(TFG));
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


    [HttpGet("tfgLine/{tfgLineId}")]
    public IActionResult GetTFGsByTFGLine(int tfgLineId)
    {
        TFGManager manager = new TFGManager(context);
        return Ok(manager.GetTFGsByTFGLine(tfgLineId));
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            TFGManager manager = new TFGManager(context);
            return Ok(manager.GetTFGById(id));
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

    [HttpPost("request")]
    public async Task<IActionResult> Request([FromBody] TFGRequest request)
    {
        try
        {
            TFGManager manager = new TFGManager(context, emailService, configuration);
            await manager.RequestTFG(request);
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

    [HttpGet("professor-pending/{id}")]
    public IActionResult GetPendingTFGsByProfessor(int id)
    {
        try
        {
            TFGManager manager = new TFGManager(context);
            return Ok(manager.GetPendingTFGsByProfessor(id));
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

    [HttpPost("accept/{id}")]
    public async Task<IActionResult> Accept(int id)
    {
        try
        {
            TFGManager manager = new TFGManager(context, emailService, configuration);
            await manager.AcceptTFG(id);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }

    [HttpPost("reject/{id}")]
    public async Task<IActionResult> Reject(int id)
    {
        try
        {
            TFGManager manager = new TFGManager(context, emailService, configuration);
            await manager.RejectTFG(id);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UnprocessableException e)
        {
            return UnprocessableEntity(e.GetError());
        }
    }
}
