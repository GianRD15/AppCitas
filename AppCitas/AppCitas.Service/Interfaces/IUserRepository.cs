using AppCitas.Service.Entities;

namespace AppCitas.Service.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetUserAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<bool> SaveAllAsync();
    void Update(AppUser user);
}
