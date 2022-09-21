using AppCitas.Service.Data;
using AppCitas.Service.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCitas.Service.Controllers;

public class BuggyController: BaseApiController
{
    private readonly DataContext _context;

    public BuggyController (DataContext context)
    {
        _context = context;
    }

    [HttpGet("auth")]
    [Authorize]
    public ActionResult<string> GetSecret()
    {
        return "Secret Text";
    }

    [HttpGet("not_found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = _context.Users.Find(-1);
        if (thing == null) return NotFound();
        return Ok(thing);
    }

    [HttpGet("server_error")]
    public ActionResult<string> GetServerError()
    {
        var thing = _context.Users.Find(-1);
        var thingToReturn = thing.ToString();

        return thingToReturn;
    }

    [HttpGet("bad_request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This not a good request");
    }
}
