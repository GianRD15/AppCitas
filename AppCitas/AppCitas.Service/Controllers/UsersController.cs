using AppCitas.Service.Data;
using AppCitas.Service.Entities;
using AppCitas.Service.Entities.DOTs;
using AppCitas.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AppCitas.Service.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository= userRepository;
        _mapper= mapper;
    }
    // GET api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var users = await _userRepository.GetMembersAsync();

        return Ok(users);
    }

    // GET api/users/{id}

    //[HttpGet("{id}")]
    //public async Task<ActionResult<AppUser>> GetUserById(int id)
    //{
    //    return await _userRepository.GetUserByIdAsync(id);
    //}

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDTO>> GetUserByUsername(string username)
    {
        return await _userRepository.GetMemberAsync(username);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO member)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUserByUsernameAsync(username);

        _mapper.Map(member,user);

        _userRepository.Update(user);

        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");
    }

}
