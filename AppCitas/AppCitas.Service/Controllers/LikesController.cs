using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.Extensions;
using AppCitas.Service.Helpers;
using AppCitas.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppCitas.Service.Controllers;
[Authorize]
public class LikesController : BaseApiController
{
	private readonly IUserRepository _userRepository;
	private readonly ILikesRepository _likesRepository;

	public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
	{
		_userRepository = userRepository;
		_likesRepository = likesRepository;
    }

	[HttpPost("{username}")]
	public async Task<ActionResult> AddLike(string username)
	{
		var sourceUserId = User.GetUserId();
		var likedUser = await _userRepository.GetUserByUsernameAsync(username);
		var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

		if (likedUser == null) return NotFound();

		if (sourceUser.UserName.Equals(username)) return BadRequest("You cannot like yourself");

		var userlike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

		if (userlike != null) return BadRequest("You already like this user");

		userlike = new UserLike
		{
			SourceUserId = sourceUserId,
			LikedUserId = likedUser.Id
		};

		sourceUser.LikedUsers.Add(userlike);

		if(await _userRepository.SaveAllAsync()) return Ok();
		else return BadRequest("Failed to like user");
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
	{
		likesParams.UserID = User.GetUserId();
		var users = await _likesRepository.GetUserLikes(likesParams);

		Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

		return Ok(users);
	}
}
