﻿using AppCitas.Service.Data;
using AppCitas.Service.Entities;
using AppCitas.Service.Entities.DOTs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AppCitas.Service.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    public AccountController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDto)
    {
        if( await userExist(registerDto.Username))
        {
            return BadRequest(new { message = "Username is already taken" });
        }

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    #region private methods

    private async Task<bool> userExist(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }

    #endregion
}
