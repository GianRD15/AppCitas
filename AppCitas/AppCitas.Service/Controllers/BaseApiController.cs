using AppCitas.Service.Helpers;
using Microsoft.AspNetCore.Mvc;
namespace AppCitas.Service.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BaseApiController : ControllerBase
{

}
