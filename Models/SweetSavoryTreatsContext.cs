using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SweetSavoryTreats.Models
{
  public class SweetSavoryTreatsContext : IdentityDbContext<User>
  {
    public DbSet<Taste> Flavors { get; set; }
    public DbSet<Treat> Treats { get; set; }
    public DbSet<TasteTreat> FlavorTreat { get; set; }
    public SweetSavoryTreatsContext(DbContextOptions options) : base(options) { }
  }
}