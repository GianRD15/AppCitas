using AppCitas.Service.Entities;
using AppCitas.Service.Entities.DOTs;
using AppCitas.Service.Helpers;

namespace AppCitas.Service.Interfaces;

public interface IUserRepository
{
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<bool> SaveAllAsync();
    void Update(AppUser user);

    Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);
    Task<MemberDTO> GetMemberAsync(string username);
}
