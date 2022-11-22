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

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = _context.Users.Find(-1);
        return NotFound();
        
    }

    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        var thing = _context.Users.Find(-1);
        var thing2="";
        return thing2 = thing.ToString();

    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This not a good request");
    }
}
