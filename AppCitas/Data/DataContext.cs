using AppCitas.service.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppCitas.service.Data


{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
    }
}
