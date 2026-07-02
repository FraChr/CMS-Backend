using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class Context : DbContext
{
    public Context (DbContextOptions<Context> options) : base (options) { }
    
    public DbSet<Certificate> Certificates { get; set; }
    
}