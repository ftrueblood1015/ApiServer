using Domain.Entities.Application;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        DbSet<User> Users => Set<User>();
        DbSet<Role> Roles => Set<Role>();
        DbSet<UserRole> UserRole => Set<UserRole>();
    }
}
