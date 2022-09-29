﻿using AppCitas.Service.Entities;
using AppCitas.Service.Entities.DOTs;
using AppCitas.Service.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AppCitas.Service.Data;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public UserRepository(DataContext contex, IMapper mapper)
    {
        _context = contex;
        _mapper = mapper;
    }

    public async Task<MemberDTO> GetMemberAsync(string username)
    {
        return await _context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public Task<IEnumerable<MemberDTO>> GetMembersAsync()
    {
        throw new NotImplementedException();
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
