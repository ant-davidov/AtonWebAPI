using AronWebAPI.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AronWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();

        }
    }
}

