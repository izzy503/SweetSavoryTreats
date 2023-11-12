using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SweetSavoryTreats.Models
{
  public class SweetSavoryTreatsContext : IdentityDbContext<User>
  {
    public DbSet<Taste> Taste { get; set; }
    public DbSet<Treat> Treats { get; set; }
    public DbSet<TasteTreat>TasteTreat { get; set; }
    public SweetSavoryTreatsContext(DbContextOptions options) : base(options) { }
  }
}