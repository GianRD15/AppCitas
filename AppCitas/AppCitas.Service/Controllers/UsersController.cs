using AppCitas.Service.Data;
using AppCitas.Service.Entities;
using AppCitas.Service.Entities.DOTs;
using AppCitas.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppCitas.Service.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository= userRepository;
    }
    // GET api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        return Ok(await _userRepository.GetUserAsync());
    }

    // GET api/users/{id}

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUserById(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    [HttpGet("username/{id}")]
    public async Task<ActionResult<AppUser>> GetUserByUsername(string username)
    {
        Console.WriteLine(username);
        return await _userRepository.GetUserByUsernameAsync(username);
    }

}
