using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;
namespace MinimalApi.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }

    }
}
