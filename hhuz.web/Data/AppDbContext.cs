using Microsoft.EntityFrameworkCore;
using hhuz.web.Models;

namespace hhuz.web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TestR> TestPings { get; set; }
    public DbSet<TestR> Users { get; set; }
}