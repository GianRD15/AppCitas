using AppCitas.Service.Data;
using AppCitas.Service.Entities;
using AppCitas.Service.Entities.DOTs;
using AppCitas.Service.Extensions;
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
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _photoService = photoService;
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

    [HttpGet("{username}", Name = "GetUser")]
    public async Task<ActionResult<MemberDTO>> GetUserByUsername(string username)
    {
        return await _userRepository.GetMemberAsync(username);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO member)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        _mapper.Map(member, user);

        _userRepository.Update(user);

        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (user.Photos.Count == 0) photo.IsMain = true;

        user.Photos.Add(photo);

        if (await _userRepository.SaveAllAsync())
        {
            //return _mapper.Map<PhotoDTO>(photo);
            return CreatedAtRoute("GetUser", new {username = user.UserName }, _mapper.Map<PhotoDTO>(photo));
        }

        return BadRequest("Problem adding photo");

    }
}
