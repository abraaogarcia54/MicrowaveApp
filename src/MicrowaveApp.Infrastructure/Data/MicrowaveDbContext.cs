

using Microsoft.EntityFrameworkCore;
using MicrowaveApp.Domain.Entities;

namespace MicrowaveApp.Infrastructure.Data;

public sealed class MicrowaveDbContext : DbContext
{
    public MicrowaveDbContext(DbContextOptions<MicrowaveDbContext> options) : base(options)
    {
    }
    
    public DbSet<HeatingProgram> HeatingPrograms => Set<HeatingProgram>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<HeatingProgram>(entity =>
        {
            entity.HasKey(program => program.Id);

            entity.Ignore(program => program.IsPreset);

            entity.Property(program => program.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(program => program.Food)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(program => program.TimeInSeconds)
                .IsRequired();

            entity.Property(program => program.Power)
                .IsRequired();

            entity.Property(program => program.HeatingChar)
                .IsRequired();

            entity.Property(program => program.Instructions)
                .HasMaxLength(1000);

            entity.Property(program => program.IsPresent)
                .IsRequired();

            entity.Property(program => program.CreatedAt)
                .IsRequired();

            entity.HasIndex(program => program.HeatingChar)
                .IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);

            entity.Property(user => user.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(user => user.PasswordHash)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(user => user.CreatedAt)
                .IsRequired();

            entity.HasIndex(user => user.Username)
                .IsUnique();
        });
    }
}
