using Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Api;

public class Context : DbContext
{
    public Context (DbContextOptions<Context> options) : base (options) { }
    
    public DbSet<Certificate> Certificates { get; set; }
}