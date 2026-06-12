

using Microsoft.EntityFrameworkCore;
using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Infrastructure.Data;

public class MicrowaveDbContext : DbContext
{
    public MicrowaveDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<HeatingProgram> HeatingPrograms => Set<HeatingProgram>();
    
    public DbSet<User>  Users => Set<User>(); 
}
