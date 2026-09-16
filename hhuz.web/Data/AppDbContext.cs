using hhuz.Models;
using Microsoft.EntityFrameworkCore;

namespace hhuz.web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Users> Users { get; set; }
    public DbSet<Positions>  Positions { get; set; }
    public DbSet<Cvs> Cvs { get; set; }
}