﻿using AppCitas.Service.Entities;
using AppCitas.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppCitas.Service.Data;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    public UserRepository(DataContext contex)
    {
        _context = contex;
    }
    public async Task<IEnumerable<AppUser>> GetUserAsync()
    {
        return await _context.Users
            .Include(p => p.Photos)
            .ToListAsync();
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified; 
    }
}
